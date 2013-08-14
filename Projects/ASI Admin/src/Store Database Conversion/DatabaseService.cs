using asi.asicentral.database;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using Store_Database_Conversion.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion
{
    public class DatabaseService : IDisposable
    {
        ASIInternetContext _asiInternetContext;
        StoreContext _storeContext;
        ILogService _logService;

        public DatabaseService(DatabaseTarget target)
        {
            _asiInternetContext = new ASIInternetContext("ASIInternetContext" + target);
            _storeContext = new StoreContext("ProductContext" + target);
            _storeContext.Configuration.ValidateOnSaveEnabled = false; //turn off the validation for importing legacy data
            _logService = LogService.GetLog(this.GetType());
        }

        public int GetLegacyCount()
        {
            var count = _asiInternetContext.Orders.Count();
            return count;
        }

        public int GetNewCount()
        {
            var count = _storeContext.StoreOrders.Count();
            return count;
        }

        private bool LegacyExists(int id)
        {
            return _storeContext.StoreOrders.Where(order => order.LegacyId == id).Count() > 0;
        }

        private bool IsTestRecord(LegacyOrder order)
        {
            bool isTestRecord = false;
            isTestRecord = isTestRecord || (order.BillCity != null && (new string[] {"trevose", "test"}).Contains(order.BillCity.ToLower()));
            isTestRecord = isTestRecord || (order.BillLastName != null && (new string[] {"driscoll", "tucker", "fairfield"}).Contains(order.BillLastName.ToLower()));
            if (!isTestRecord && order.Addresses.Count > 0)
            {
                foreach (var address in order.Addresses)
                {
                    isTestRecord = isTestRecord || (string.IsNullOrEmpty(address.SPAD_City) || address.SPAD_City.ToLower() == "trevose");
                    isTestRecord = isTestRecord || (address.SPAD_Email != null && address.SPAD_Email.ToLower().Contains("asicentral.com"));
                }
            }
            if (!isTestRecord && order.DistributorAddresses.Count > 0)
            {
                foreach (var address in order.DistributorAddresses)
                {
                    isTestRecord = isTestRecord || (string.IsNullOrEmpty(address.City) || address.City.ToLower() == "trevose");
                    isTestRecord = isTestRecord || (address.WebAdd != null && address.WebAdd.ToLower().Contains("asicentral.com"));
                }
            }
            if (!isTestRecord)
            {
                LegacyOrderContact orderContact = _asiInternetContext.OrderContacts.Where(t => t.OrderId == order.Id).FirstOrDefault();
                if (orderContact != null) isTestRecord = (orderContact.Company != null && orderContact.Company == "ASI") || (orderContact.Email != null && orderContact.Email.ToLower().Contains("asicentral.com"));
            }
            if (!isTestRecord)
            {
                IList<LegacyOrderMagazineAddress> magAddresses = _asiInternetContext.LegacyOrderMagazineAddresses.Where(t => t.OrderID == order.Id).ToList();
                foreach (var magAddress in magAddresses)
                {
                    isTestRecord = isTestRecord || (string.IsNullOrEmpty(magAddress.Address.MAGA_City) || magAddress.Address.MAGA_City.ToLower() == "trevose");
                    isTestRecord = isTestRecord || (!string.IsNullOrEmpty(magAddress.Address.MAGA_Email) && magAddress.Address.MAGA_Email.ToLower().Contains("asicentral.com"));
                }
            }
            return isTestRecord;
        }

        /// <summary>
        /// Main method processing legacy order records
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void ProcessLegacyRecords(int from, int to)
        {
            List<LegacyOrder> legacyOrders = _asiInternetContext.Orders.OrderByDescending(order => order.Id).Skip(from).Take(to - from).ToList();
            List<ContextProduct> productReference = _storeContext.Products.ToList();

            foreach (LegacyOrder order in legacyOrders)
            {
                if (!IsTestRecord(order) && !LegacyExists(order.Id))
                {
                    DateTime creationDate = order.DateCreated.HasValue ? order.DateCreated.Value : DateTime.MinValue;
                    StoreOrder newOrder = new StoreOrder()
                    {
                        ApprovedBy = "Unknown",
                        ApprovedDate = creationDate,
                        Campaign = order.Campaign,
                        CompletedStep = order.CompletedStep,
                        ContextId = order.ContextId,
                        ExternalReference = order.ExternalReference,
                        IPAdd = order.IPAdd,
                        IsCompleted = order.Status.HasValue ? order.Status.Value : false,
                        LegacyId = order.Id,
                        ProcessStatus = order.ProcessStatus,
                        CreateDate = creationDate,
                        UpdateDate = creationDate,
                        UpdateSource = "Migration Process - " + DateTime.Now,
                    };
                    _storeContext.StoreOrders.Add(newOrder);
                    //add logged in user information
                    LegacyOrderContact orderContact = _asiInternetContext.OrderContacts.Where(t => t.OrderId == order.Id).FirstOrDefault();
                    if (orderContact != null && !string.IsNullOrEmpty(orderContact.Email)) newOrder.LoggedUserEmail = orderContact.Email;
                    //add billing contact
                    newOrder.BillingIndividual = GetBillingIndividual(order);
                    //order detail records
                    bool containsSubscription = false;
                    foreach (LegacyOrderDetail detail in order.OrderDetails)
                    {
                        ContextProduct product = null;
                        int productId = ConvertProductId(detail.ProductId);
                        if (productId > 0) product = productReference.Where(prod => prod.Id == productId).SingleOrDefault();
                        StoreOrderDetail newDetail = new StoreOrderDetail()
                        {
                            Order = newOrder,
                            Product = product,
                            LegacyProductId = detail.ProductId,
                            Quantity = detail.Quantity.HasValue ? detail.Quantity.Value : 0,
                            IsSubscription = product != null && product.IsSubscription,
                            Cost = detail.PreTaxSubtotal.HasValue ? detail.PreTaxSubtotal.Value : 0.0m,
                            ShippingCost = detail.Shipping.HasValue ? detail.Shipping.Value : 0.0m,
                            TaxCost = detail.TaxSubtotal.HasValue ? detail.TaxSubtotal.Value : 0.0m,
                            ApplicationCost = detail.ApplicationFee.HasValue ? detail.ApplicationFee.Value : 0.0m,
                            CreateDate = creationDate,
                            UpdateDate = creationDate,
                            UpdateSource = "Migration Process - " + DateTime.Now,
                        };
                        containsSubscription = containsSubscription || IsSubscription(detail.ProductId);
                        newOrder.OrderDetails.Add(newDetail);
                        _storeContext.StoreOrderDetails.Add(newDetail); //some records do not have a key generated unless added explicitely
                        if (!string.IsNullOrEmpty(detail.Application) && detail.Application.ToLower() != "new")
                        {
                            //need to commit the data to make sure ids are generated
                            _storeContext.SaveChanges();
                            //assuming hallmark submission, add a new entry
                            StoreDetailHallmarkRequest hallmark = new StoreDetailHallmarkRequest()
                            {
                                OrderDetailId = newDetail.Id,
                                WebRequest = detail.Application.Length > 10000 ?  detail.Application.Substring(0, 10000) : detail.Application,
                                IsSuccessful = true,
                                CreateDate = creationDate,
                                UpdateDate = creationDate,
                                UpdateSource = "Migration Process - " + DateTime.Now,
                            };
                            _storeContext.HallmarkFormRequests.Add(hallmark);
                        }
                        //processing the applications
                        IProductConvert productConvert = GetProductConvert(detail.ProductId);
                        if (productConvert != null)
                        {
                            //need to commit the data to make sure ids are generated
                            _storeContext.SaveChanges();
                            productConvert.Convert(newDetail, detail, _storeContext, _asiInternetContext);
                        }
                    }
                    //credit card information
                    if (order.CreditCard != null)
                    {
                        LegacyOrderCreditCard creditCard = order.CreditCard;
                        StoreCreditCard newCreditCard = new StoreCreditCard()
                        {
                            CardHolderName = creditCard.Name,
                            CardNumber = creditCard.Number,
                            CardType = creditCard.Type,
                            ExpMonth = creditCard.ExpMonth,
                            ExpYear = creditCard.ExpYear,
                            ExternalReference = creditCard.ExternalReference,
                            CreateDate = creationDate,
                            UpdateDate = creationDate,
                            UpdateSource = "Migration Process - " + DateTime.Now,
                        };
                        if (order.CreditCard.TotalAmount.HasValue)
                        {
                            newOrder.Total = order.CreditCard.TotalAmount.Value;
                            //create an annualized value if any of the order details record contain a subscription. This should be accurate for most of them.
                            if (containsSubscription) newOrder.AnnualizedTotal = newOrder.Total * 12;
                        }
                        _storeContext.StoreCreditCards.Add(newCreditCard); //keetp this here or EF is getting confused
                        newOrder.CreditCard = newCreditCard;
                    }
                    //first start with company record - there is not more than one record per order in the legacy database
                    if (order.DistributorAddresses.Count > 1)
                    {
                        StoreCompany company = newOrder.Company;
                        if (company == null)
                        {
                            company = new StoreCompany() { Name = order.DistributorAddresses[0].Name, };
                            _storeContext.StoreCompanies.Add(company);
                            newOrder.Company = company;
                        }
                        StoreAddress address = new StoreAddress()
                        {
                            Street1 = order.DistributorAddresses[0].Address,
                            City = order.DistributorAddresses[0].City,
                            Zip = order.DistributorAddresses[0].Zip,
                            State = order.DistributorAddresses[0].State,
                            Phone = order.DistributorAddresses[0].Phone,
                            CreateDate = creationDate,
                            UpdateDate = creationDate,
                            UpdateSource = "Migration Process - " + DateTime.Now,
                        };
                        company.WebURL = company.WebURL == null ? order.DistributorAddresses[0].WebAdd : company.WebURL;
                        _storeContext.StoreAddresses.Add(address);
                        StoreCompanyAddress companyAddressItem = new StoreCompanyAddress()
                        {
                            Address = address,
                            IsBilling = false,
                            IsShipping = true,
                            CreateDate = creationDate,
                            UpdateDate = creationDate,
                            UpdateSource = "Migration Process - " + DateTime.Now,
                        };
                        _storeContext.StoreCompanyAddresses.Add(companyAddressItem);
                        company.Addresses.Add(companyAddressItem);

                        if (!string.IsNullOrEmpty(order.DistributorAddresses[0].Contact) && 
                            company.Individuals.Where(t => t.FirstName + " " + t.LastName == order.DistributorAddresses[0].Contact).Count() == 0)
                        {
                            string[] nameParts = order.DistributorAddresses[0].Contact.Split(' ');
                            //add the additional contact
                            StoreIndividual contact = new StoreIndividual()
                            {
                                FirstName = nameParts[0].Trim(),
                                LastName = (nameParts.Length > 0 ? nameParts[nameParts.Length - 1].Trim() : nameParts[0].Trim()),
                                Phone = order.DistributorAddresses[0].Phone,
                                CreateDate = creationDate,
                                UpdateDate = creationDate,
                                UpdateSource = "Migration Process - " + DateTime.Now,
                            };
                            _storeContext.StoreIndividuals.Add(contact);
                            company.Individuals.Add(contact);
                        }
                    }
                }
                else
                {
                    _logService.Debug("Order is already present in target database: " + order.Id);
                }
            }
            _storeContext.SaveChanges();
        }

        //extracts the billing address from the order
        private static StoreIndividual GetBillingIndividual(LegacyOrder order)
        {
            //go through order detail and find out from the product item the billing address
            StoreAddress address = null;
            StoreIndividual individual = null;
            if (order.BillCity != null)
            {
                address = new StoreAddress()
                {
                    Street1 = order.BillStreet1,
                    Street2 = order.BillStreet2,
                    City = order.BillCity,
                    State = order.BillState,
                    Zip = order.BillZip,
                    Country = order.BillCountry,
                    CreateDate = order.DateCreated.HasValue ? order.DateCreated.Value : DateTime.MinValue,
                    UpdateDate = order.DateCreated.HasValue ? order.DateCreated.Value : DateTime.MinValue,
                    UpdateSource = "Migration Process - " + DateTime.Now,
                };
            }
            if (!string.IsNullOrEmpty(order.BillFirstName) || !string.IsNullOrEmpty(order.BillLastName) || address != null)
            {
                individual = new StoreIndividual()
                {
                    FirstName = order.BillFirstName,
                    LastName = order.BillLastName,
                    IsPrimary = true,
                    Phone = order.BillPhone,
                    CreateDate = order.DateCreated.HasValue ? order.DateCreated.Value : DateTime.MinValue,
                    UpdateDate = order.DateCreated.HasValue ? order.DateCreated.Value : DateTime.MinValue,
                    UpdateSource = "Migration Process - " + DateTime.Now,
                    Address = address,
                };
            }
            return individual;
        }
        private bool IsSubscription(int productId)
        {
            return new int[] { 1, 103, 205, 206, 212, 3, 195, 102, 152, 7, 8, 9, 10, 11, 104, 166 }.Contains(productId);
        }

        private int ConvertProductId(int productId)
        {
            int newProductId = 0;
            switch (productId)
            {
                //Distributor
                case 1:
                case 103:
                    newProductId = 5;
                    break;
                case 205:
                    newProductId = 6;
                    break;
                case 206:
                    newProductId = 7;
                    break;
                case 212:
                    newProductId = 8;
                    break;
                //supplier
                case 3:
                    newProductId = 1;
                    break;
                case 195:
                    newProductId = 2;
                    break;
                case 102:
                    newProductId = 3;
                    break;
                case 152:
                    newProductId = 4;
                    break;
                case 7: //Counselor
                    newProductId = 25;
                    break;
                case 8: //advantages
                    newProductId = 26;
                    break;
                case 10: //stitches
                    newProductId = 27;
                    break;
                case 11: //wearables
                    newProductId = 28;
                    break;
                case 9: //Successful Promotions Magazine
                    newProductId = 32;
                    break;
                case 164: //Successful Promotions Magazine Special Offer
                    newProductId = 33;
                    break;
                case 166: //Successful Promotions Magazine Special Offer Dallas
                    newProductId = 34;
                    break;
                case 104: //supplier fees
                    break;
                //catalog products
                case 140:
                    newProductId = 42;
                    break;
                case 141:
                    newProductId = 40;
                    break;
                case 142:
                case 160:
                    newProductId = 39;
                    break;
                case 143:
                    newProductId = 41;
                    break;
                case 144:
                case 208:
                    newProductId = 43;
                    break;
                case 145:
                case 161:
                    newProductId = 38;
                    break;
                case 146:
                case 162:
                    newProductId = 35;
                    break;
                case 147:
                    newProductId = 37;
                    break;
                case 163:
                    newProductId = 44;
                    break;
                case 192:
                case 209:
                    newProductId = 36;
                    break;

                default:
                    _logService.Error("There is a product which has not been mapped yet: " + productId);
                    break;
            }
            return newProductId;
        }

        private IProductConvert GetProductConvert(int productId)
        {
            switch (productId)
            {
                //Distributor
                case 1:
                case 103:                    
                case 205:
                case 206:
                case 212:
                    return new DistributorMembership();
                //supplier
                case 3:
                case 195:
                case 102:
                case 152:
                    return new SupplierMembership();
                case 104: //supplier fees
                    return null;
                case 7: //Magazines
                case 8:
                case 10:
                case 11:
                case 9: //Successful Promotions
                case 164:
                case 166:
                    return new Magazines();
                case 140: //Catalogs
                case 141:
                case 142:
                case 143:
                case 144:
                case 145:
                case 146:
                case 147:
                case 160:
                case 161:
                case 162:
                case 163:
                case 192:
                case 208:
                case 209:
                    return new Catalog();
                default:
                    _logService.Error("There is a product which has not been mapped yet: " + productId);
                    return null;
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                _asiInternetContext.Dispose();
                _storeContext.Dispose();
            }
            //no unmanaged resource to free at this point
        }

        #endregion IDisposable
    }
}
