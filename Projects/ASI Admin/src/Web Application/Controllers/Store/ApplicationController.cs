using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.model.store;
using asi.asicentral.services;

namespace asi.asicentral.web.Controllers.Store
{
    [Authorize]
    public class ApplicationController : Controller
    {
        public const string COMMAND_SAVE = "Save";
        public const string COMMAND_REJECT = "Reject";
        public const string COMMAND_ACCEPT = "Accept";

        public IStoreService StoreService { get; set; }
        public IFulfilmentService FulfilmentService { get; set; }
        public ICreditCardService CreditCardService { get; set; }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>(true).Where(detail => detail.Id == id).FirstOrDefault();
            if (orderDetail == null) throw new Exception("Invalid Order Detail Id");
            StoreDetailApplication application = null;
            if (orderDetail != null) application = StoreService.GetApplication(orderDetail);
            if (application != null)
            {
                if (application is StoreDetailSupplierMembership) return View("../Store/Application/Supplier", new SupplierApplicationModel((StoreDetailSupplierMembership)application, orderDetail));
                else if (application is StoreDetailDistributorMembership) return View("../Store/Application/Distributor", new DistributorApplicationModel((StoreDetailDistributorMembership)application, orderDetail));
                else throw new Exception("Retieved an unknown type of application");
            }
            else
            {
                if (orderDetail.Product != null && orderDetail.Product.Type == "Magazine") return View("../Store/Application/Magazines", new MagazinesApplicationModel(orderDetail));
                else throw new Exception("Retieved an unknown type of application");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditDistributor(DistributorApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                StoreDetailDistributorMembership distributorApplication = StoreService.GetAll<StoreDetailDistributorMembership>().Where(app => app.OrderDetailId == application.OrderDetailId).SingleOrDefault();
                if (order == null) throw new Exception("Invalid reference to an order");
                if (distributorApplication == null) throw new Exception("Invalid reference to an application");
                order.ExternalReference = application.ExternalReference;

                //view does not contain some of the collections, copy from the ones in the database
                application.SyncAccountTypesToApplication(StoreService.GetAll<LookDistributorAccountType>().ToList(), distributorApplication);
                application.SyncProductLinesToApplication(StoreService.GetAll<LookProductLine>().ToList(), distributorApplication);
                application.ProductLines = distributorApplication.ProductLines;
                application.AccountTypes = distributorApplication.AccountTypes;

                LookDistributorRevenueType PrimaryBusinessRevenue = StoreService.GetAll<LookDistributorRevenueType>(false).Where(revenue => revenue.Name == application.BuisnessRevenue).SingleOrDefault();
                if (PrimaryBusinessRevenue != null)
                {
                    application.PrimaryBusinessRevenue = PrimaryBusinessRevenue;
                }
                else
                {
                    application.PrimaryBusinessRevenue = null;
                }
                order = UpdateCompanyInformation(application, order);
                application.CopyTo(distributorApplication);

                ProcessCommand(StoreService, FulfilmentService, order, distributorApplication, application.ActionName);
                distributorApplication.UpdateDate = DateTime.UtcNow;
                distributorApplication.UpdateSource = "ASI Admin Application - EditDistributor";
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Distributor", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditSupplier(SupplierApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                StoreDetailSupplierMembership supplierApplication = StoreService.GetAll<StoreDetailSupplierMembership>(false).Where(app => app.OrderDetailId == application.OrderDetailId).SingleOrDefault();
                if (order == null) throw new Exception("Invalid reference to an order");
                if (supplierApplication == null) throw new Exception("Invalid reference to an application");
                order.ExternalReference = application.ExternalReference;
                //copy decorating types bool to the collections
                application.SyncDecoratingTypes(StoreService.GetAll<LookSupplierDecoratingType>().ToList(), supplierApplication);
                application.DecoratingTypes = supplierApplication.DecoratingTypes;
                order = UpdateCompanyInformation(application, order);
                application.CopyTo(supplierApplication);
                supplierApplication.UpdateDate = DateTime.UtcNow;
                supplierApplication.UpdateSource = "ASI Admin Application - EditSupplier";
                ProcessCommand(StoreService, FulfilmentService, order, supplierApplication, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Supplier", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditMagazines(MagazinesApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                if (application.ProductName != "Stitches" && application.ProductName != "Wearables")
                {
                    order = UpdateCompanyInformation(application, order);
                }
                orderDetail = UpdateMagazineSubscriptionInformation(application, orderDetail);
                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Magazines", application);
            }
        }

        /// <summary>
        /// Common code between Edit supplier and distributor
        /// </summary>
        /// <param name="storeService"></param>
        /// <param name="order"></param>
        /// <param name="applicationId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private void ProcessCommand(IStoreService storeService, IFulfilmentService fulfilmentService, StoreOrder order, StoreDetailApplication application, string command)
        {
            if (command == ApplicationController.COMMAND_ACCEPT)
            {
                //make sure we have external reference
                if (string.IsNullOrEmpty(order.ExternalReference)) throw new Exception("You need to specify a Timms id to approve an order");

                //make sure timms id contains numbers only
                int num;
                bool success = int.TryParse(order.ExternalReference, out num);
                if (!success) throw new Exception("Timms id must be numbers only.");
                
                fulfilmentService.Process(order, application);
                order.ProcessStatus = OrderStatus.Approved;
            }
            else if (command == ApplicationController.COMMAND_REJECT)
            {
                order.ProcessStatus = OrderStatus.Rejected;
                try
                {
                    if (CreditCardService != null && order.CreditCard != null && !string.IsNullOrEmpty(order.CreditCard.ExternalReference))
                        CreditCardService.Delete(order.CreditCard.ExternalReference);
                }
                catch (Exception exception)
                {
                    ILogService log = LogService.GetLog(this.GetType());
                    log.Error("Could not remove a credit card record: " + exception.Message);
                }
                order.CreditCard.ExternalReference = null;
            }
        }

        /// <summary>
        /// UpdateCompanyInformation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private StoreOrder UpdateCompanyInformation(IMembershipModel model, StoreOrder order)
        {
            if (order == null || model == null) return null;
            //Update in company fields
            if (model.Company != null && order.Company != null)
            {
                order.Company.Name = model.Company;
                order.Company.Phone =model.Phone;
                order.Company.WebURL = model.BillingWebUrl;
                order.UpdateDate = DateTime.UtcNow;
                order.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";

                StoreCompanyAddress companyAddress = order.Company.Addresses.Where(add => !add.IsShipping && !add.IsBilling).FirstOrDefault();
                if (companyAddress != null && CompareAddresses(companyAddress.Address, model.Address1, model.Address2, model.State, model.City,model.Country, model.Zip))
                {
                    companyAddress.Address.Street1 = model.Address1;
                    companyAddress.Address.Street2 = model.Address2;
                    companyAddress.Address.City = model.City;
                    companyAddress.Address.Zip = model.Zip;
                    companyAddress.Address.State = model.State;
                    companyAddress.Address.Country = model.Country;
                    companyAddress.Address.UpdateDate = DateTime.UtcNow;
                    companyAddress.Address.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                }
                //Set contact information
                if (model.Contacts != null)
                {
                    int i = 0;
                    foreach(StoreIndividual individual in model.Contacts)
                    {
                        order.Company.Individuals.ElementAt(i).IsPrimary = individual.IsPrimary;
                        order.Company.Individuals.ElementAt(i).FirstName = individual.FirstName;
                        order.Company.Individuals.ElementAt(i).LastName = individual.LastName;
                        order.Company.Individuals.ElementAt(i).Email = individual.Email;
                        order.Company.Individuals.ElementAt(i).Title = individual.Title;
                        order.Company.Individuals.ElementAt(i).Phone = individual.Phone;
                        order.Company.Individuals.ElementAt(i).Fax = individual.Fax;
                        order.Company.Individuals.ElementAt(i).UpdateDate = DateTime.UtcNow;
                        order.Company.Individuals.ElementAt(i).UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                        i++;
                    }
                }
            
                //Set billing information
                if (order.BillingIndividual != null)
                {
                    if(CompareIndividuals(order.BillingIndividual, model.BillingPhone, model.BillingFax, model.BillingEmail))
                    {
                        order.BillingIndividual.Email = model.BillingEmail;
                        order.BillingIndividual.Fax = model.BillingFax;
                        order.BillingIndividual.Phone = model.BillingPhone;
                        order.BillingIndividual.UpdateDate = DateTime.UtcNow;
                        order.BillingIndividual.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                    }
                    if (order.BillingIndividual.Address != null)
                    {
                        if (CompareAddresses(order.BillingIndividual.Address, model.BillingAddress1, model.BillingAddress2, model.BillingState, model.BillingCity, model.BillingCountry, model.BillingZip))
                        {
                            if (companyAddress.Address.Id == order.BillingIndividual.Address.Id)
                            {
                                //address was not yet in use, create a new one for both individual and company
                                order.BillingIndividual.Address = new StoreAddress()
                                {
                                    CreateDate = DateTime.UtcNow,
                                };
                                order.Company.Addresses.Add(new StoreCompanyAddress()
                                {
                                    Address = order.BillingIndividual.Address,
                                    IsBilling = true,
                                    CreateDate = DateTime.UtcNow,
                                    UpdateDate = DateTime.UtcNow,
                                    UpdateSource = "ASI Admin Application - UpdateCompanyInformation",
                                });
                                StoreService.Add<StoreAddress>(order.BillingIndividual.Address);
                            }
                            order.BillingIndividual.Address.Street1 = model.BillingAddress1;
                            order.BillingIndividual.Address.Street2 = model.BillingAddress1;
                            order.BillingIndividual.Address.City = model.BillingCity;
                            order.BillingIndividual.Address.State = model.BillingState;
                            order.BillingIndividual.Address.Zip = model.BillingZip;
                            order.BillingIndividual.Address.Country = model.BillingCountry;
                            order.BillingIndividual.Address.UpdateDate = DateTime.UtcNow;
                            order.BillingIndividual.Address.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                        }
                    }
                }
                //Set shipping information
                if (model.HasShipAddress)
                {
                    StoreCompanyAddress address = order.Company.Addresses.Where(add => add.IsShipping).FirstOrDefault();
                    if (address != null && address.Address != null && CompareAddresses(address.Address, model.ShippingStreet1, model.ShippingStreet2, model.ShippingState, model.ShippingCity, model.ShippingCountry, model.ShippingZip))
                    {
                        address.Address.Street1 = model.ShippingStreet1;
                        address.Address.Street2 = model.ShippingStreet2;
                        address.Address.City = model.ShippingCity;
                        address.Address.Zip = model.ShippingZip;
                        address.Address.State = model.ShippingState;
                        address.Address.Country = model.ShippingCountry;
                        address.Address.UpdateDate = DateTime.UtcNow;
                        address.Address.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                    }
                }
            }
            return order;
        }

        private bool CompareIndividuals(StoreIndividual individual1, string phone, string fax, string email)
        {
            bool isChangesInInidividuals = false;

            if (individual1 == null || individual1.Phone != phone || individual1.Fax != fax || individual1.Email != email)
                isChangesInInidividuals = true;
            else
                isChangesInInidividuals = false;

            return isChangesInInidividuals;
        }

        private bool CompareAddresses(StoreAddress address1, string street1, string street2, string state, string city, string country, string zip)
        {
            bool isChangesInAddress = false;

            if (address1 == null || address1.Street1 != street1 || address1.Street2 != street2 || address1.State != state || address1.City != city || address1.Country != country || address1.Zip != zip)
                isChangesInAddress = true;
            else
                isChangesInAddress = false;

            return isChangesInAddress;
        }

        private StoreOrderDetail UpdateMagazineSubscriptionInformation(MagazinesApplicationModel application, StoreOrderDetail orderDetail)
        {
            //copy decorating types bool to the collections
            foreach (StoreMagazineSubscription subscription in application.Subscriptions)
            {
                StoreMagazineSubscription existing = orderDetail.MagazineSubscriptions.Where(item => item.Id == subscription.Id).SingleOrDefault();
                if (subscription.Contact != null && existing != null)
                {
                    existing.CompanyName = subscription.CompanyName;
                    existing.ASINumber = subscription.ASINumber;
                    existing.IsDigitalVersion = subscription.IsDigitalVersion;
                    existing.UpdateDate = DateTime.UtcNow;
                    existing.UpdateSource = "ASI Admin Application - EditMagazines";
                    StoreService.Update<StoreMagazineSubscription>(existing);

                    if (subscription.Contact != null && existing.Contact != null)
                    {
                        existing.Contact.FirstName = subscription.Contact.FirstName;
                        existing.Contact.LastName = subscription.Contact.LastName;
                        existing.Contact.Email = subscription.Contact.Email;
                        existing.Contact.Title = subscription.Contact.Title;
                        existing.Contact.Phone = subscription.Contact.Phone;
                        existing.Contact.Fax = subscription.Contact.Fax;
                        existing.Contact.Department = subscription.Contact.Department;
                        existing.Contact.UpdateDate = DateTime.UtcNow;
                        existing.Contact.UpdateSource = "ASI Admin Application - EditMagazines";
                        StoreService.Update<StoreIndividual>(existing.Contact);
                    }

                    if (subscription.Contact.Address != null && existing.Contact.Address != null)
                    {
                        existing.Contact.Address.Street1 = subscription.Contact.Address.Street1;
                        existing.Contact.Address.Street2 = subscription.Contact.Address.Street2;
                        existing.Contact.Address.City = subscription.Contact.Address.City;
                        existing.Contact.Address.State = subscription.Contact.Address.State;
                        existing.Contact.Address.Zip = subscription.Contact.Address.Zip;
                        existing.Contact.Address.Country = subscription.Contact.Address.Country;
                        existing.Contact.Address.UpdateDate = DateTime.UtcNow;
                        existing.Contact.Address.UpdateSource = "ASI Admin Application - EditMagazines";
                        StoreService.Update<StoreAddress>(existing.Contact.Address);
                    }
                }
            }
            orderDetail.UpdateDate = DateTime.UtcNow;
            orderDetail.UpdateSource = "ASI Admin Application - EditMagazines";
            StoreService.Update<StoreOrderDetail>(orderDetail);
            return orderDetail;
        }
    }
}