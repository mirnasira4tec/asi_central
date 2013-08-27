using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class StoreService : ObjectService, IStoreService
    {
        public StoreService(IContainer container)
            : base(container)
        {
            //nothing to do right now
        }

        public override IQueryable<T> GetAll<T>(bool readOnly = false)
        {
            return base.GetAll<T>(readOnly);
        }

        public virtual StoreDetailDistributorMembership GetDistributorApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailDistributorMembership application = null;
            if (orderDetail.Product != null && StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
                application = GetAll<StoreDetailDistributorMembership>().Where(app => app.OrderDetailId == orderDetail.Id).SingleOrDefault();
            }
            return application;
        }

        public virtual model.store.StoreDetailSupplierMembership GetSupplierApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailSupplierMembership application = null;
            if (orderDetail.Product != null && StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
                application = GetAll<StoreDetailSupplierMembership>().Where(app => app.OrderDetailId == orderDetail.Id).SingleOrDefault();
            }
            return application;
        }


        /// <summary>
        /// Retrieves the application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        public StoreDetailApplication GetApplication(StoreOrderDetail orderDetail)
        {
            StoreDetailApplication application = null;
            if (orderDetail.Product != null)
            {
                if (StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id)) return GetSupplierApplication(orderDetail);
                else if (StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id)) return GetDistributorApplication(orderDetail);
            }
            return application;
        }

        /// <summary>
        /// Creates an application based on the order detail information
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public StoreDetailApplication CreateApplication(StoreOrderDetail orderDetail)
        {
            StoreDetailApplication application = null;
            bool added = false;
            //make sure order detail has been saved
            //id is needed to create the reference in the application
            if (orderDetail.Id == 0) SaveChanges();

            if (orderDetail.Product != null)
            {
                if (StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id))
                {
                    //make sure the application does not already exist
                    StoreDetailSupplierMembership supplierApplication = GetAll<StoreDetailSupplierMembership>(true).Where(supp => supp.OrderDetailId == orderDetail.Id).FirstOrDefault();
                    if (supplierApplication == null)
                    {
                        added = true;
                        supplierApplication = new StoreDetailSupplierMembership();
                        Add<StoreDetailSupplierMembership>(supplierApplication);
                        application = supplierApplication;
                    }
                }
                else if (StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id))
                {
                    //make sure the application does not already exist
                    StoreDetailDistributorMembership distributorApplication = GetAll<StoreDetailDistributorMembership>(true).Where(supp => supp.OrderDetailId == orderDetail.Id).FirstOrDefault();
                    if (distributorApplication == null)
                    {
                        added = true;
                        distributorApplication = new StoreDetailDistributorMembership();
                        Add<StoreDetailDistributorMembership>(distributorApplication);
                        application = distributorApplication;
                    }
                }
                if (added)
                {
                    application.OrderDetailId = orderDetail.Id;
                    application.CreateDate = DateTime.UtcNow;
                    application.UpdateDate = DateTime.UtcNow;
                    application.UpdateSource = "StoreService - CreateApplication";
                }
            }
            return application;
        }

        /// <summary>
        /// Provide the Product Shipping cost
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public decimal GetShippingCost(ContextProduct product, string country, string shippingMethod = null, int quantity = 1)
        {
            decimal cost = 0m;
            if (product == null || country == null) throw new Exception("Invalid call to the GetShippingCost method");
            if (product.HasShipping)
            {
                if (product.Weight != null && !string.IsNullOrEmpty(product.Origin))
                {
                    if (shippingMethod == null) throw new Exception("You need to pass a shipping method for this product");
                    LookProductShippingRate productShippingrate = this.GetAll<LookProductShippingRate>()
                        .Where(rate => rate.Country == country && rate.Origin == product.Origin && rate.ShippingMethod == shippingMethod)
                        .FirstOrDefault();
                    if (productShippingrate == null) throw new Exception("We could not find a valid option for the GetShippingCost");
                    cost = productShippingrate.BaseAmount + (quantity * product.Weight.Value * productShippingrate.AmountOrPercent);
                }
                else if (country == "USA")
                {
                    cost = product.ShippingCostUS * quantity;
                }
                else
                {
                    cost = product.ShippingCostOther * quantity;
                }
            }
            return cost;
        }

        /// <summary>
        /// Calculates the taxes in case shipping to the USA
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal CalculateTaxes(StoreAddress address, decimal? amount)
        {
            decimal tax = 0m;
            if (address != null && address.Country == "USA" && amount != null && amount.Value > 0)
            {
                decimal taxRate = 0m;
                //look for a state record
                TaxRate taxRateRecord = this.GetAll<TaxRate>().Where(taxRecord => taxRecord.State == address.State && taxRecord.Zip == null).SingleOrDefault();
                if (taxRateRecord != null)
                {
                    taxRate = taxRateRecord.Rate;
                }
                else
                {
                    int zipCode = 0;
                    int.TryParse(address.Zip, out zipCode);
                    if (zipCode > 0)
                    {
                        //look for a state/zip record
                        taxRateRecord = this.GetAll<TaxRate>().Where(taxRecord => taxRecord.State == address.State && taxRecord.Zip == zipCode).SingleOrDefault();
                        if (taxRateRecord != null) taxRate = taxRateRecord.Rate;
                    }
                }
                if (taxRate > 0) tax = (amount.Value * taxRate) / 100m;
            }
            tax = Math.Round(tax, 2);
            return tax;
        }
    }
}
