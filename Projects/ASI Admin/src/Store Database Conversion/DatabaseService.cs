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
            return isTestRecord;
        }

        /// <summary>
        /// Main method processing legacy order records
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void ProcessLegacyRecords(int from, int to)
        {
            List<LegacyOrder> legacyOrders = _asiInternetContext.Orders.OrderBy(order => order.Id).Skip(from).Take(to - from).ToList();
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

                    //@todo order total amount - not adjusted for subscriptions
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
                        _storeContext.StoreCreditCards.Add(newCreditCard); //keetp this here or EF is getting confused
                        newOrder.CreditCard = newCreditCard;
                    }
                    //add billing contact
                    newOrder.BillingIndividual = GetBillingIndividual(order);
                    //order detail records
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
                        newOrder.OrderDetails.Add(newDetail);
                        _storeContext.StoreOrderDetails.Add(newDetail); //some records do not have a key generated unless added explicitely
                        //processing the applications
                        IProductConvert productConvert = GetProductConvert(detail.ProductId);
                        if (productConvert != null)
                        {
                            //need to commit the data to make sure ids are generated
                            _storeContext.SaveChanges();
                            productConvert.Convert(newDetail, detail, _storeContext, _asiInternetContext);
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
                case 104: //supplier fees
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
