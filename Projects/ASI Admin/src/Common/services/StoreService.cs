using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.util.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class StoreService : ObjectService, IStoreService
    {
	    static StoreService()
	    {
			//listing objects to be cached
			objectsToCache.Add("asi.asicentral.model.store.LookSendMyAdCountryCode");
	    }

	    public StoreService(IContainer container)
            : base(container)
        {
            //nothing to do right now
        }

        public virtual StoreDetailDistributorMembership GetDistributorApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailDistributorMembership application = null;
            if (orderDetail.Product != null && StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
				application = GetAll<StoreDetailDistributorMembership>().SingleOrDefault(app => app.OrderDetailId == orderDetail.Id);
            }
            return application;
        }

        public virtual model.store.StoreDetailSupplierMembership GetSupplierApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailSupplierMembership application = null;
            if (orderDetail.Product != null && StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
                application = GetAll<StoreDetailSupplierMembership>().SingleOrDefault(app => app.OrderDetailId == orderDetail.Id);
            }
            return application;
        }

        public virtual model.store.StoreDetailDecoratorMembership GetDecoratorApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailDecoratorMembership application = null;
            if (orderDetail.Product != null && StoreDetailDecoratorMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
                application = GetAll<StoreDetailDecoratorMembership>().SingleOrDefault(app => app.OrderDetailId == orderDetail.Id);
            }
            return application;
        }

        public virtual model.store.StoreDetailEquipmentMembership GetEquipmentApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailEquipmentMembership application = null;
            if (orderDetail.Product != null && StoreDetailEquipmentMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
                application = GetAll<StoreDetailEquipmentMembership>().SingleOrDefault(app => app.OrderDetailId == orderDetail.Id);
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
                else if (StoreDetailDecoratorMembership.Identifiers.Contains(orderDetail.Product.Id)) return GetDecoratorApplication(orderDetail);
                else if (StoreDetailEquipmentMembership.Identifiers.Contains(orderDetail.Product.Id)) return GetEquipmentApplication(orderDetail);
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
                    StoreDetailSupplierMembership supplierApplication = GetAll<StoreDetailSupplierMembership>(true).FirstOrDefault(supplier => supplier.OrderDetailId == orderDetail.Id);
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
                    StoreDetailDistributorMembership distributorApplication = GetAll<StoreDetailDistributorMembership>(true).FirstOrDefault(distributor => distributor.OrderDetailId == orderDetail.Id);
                    if (distributorApplication == null)
                    {
                        added = true;
                        distributorApplication = new StoreDetailDistributorMembership();
                        Add<StoreDetailDistributorMembership>(distributorApplication);
                        application = distributorApplication;
                    }
                }
                else if (StoreDetailDecoratorMembership.Identifiers.Contains(orderDetail.Product.Id))
                {
                    //make sure the application does not already exist
                    StoreDetailDecoratorMembership decoratorApplication = GetAll<StoreDetailDecoratorMembership>(true).FirstOrDefault(decorator => decorator.OrderDetailId == orderDetail.Id);
                    if (decoratorApplication == null)
                    {
                        added = true;
                        decoratorApplication = new StoreDetailDecoratorMembership();
                        Add<StoreDetailDecoratorMembership>(decoratorApplication);
                        application = decoratorApplication;
                    }
                }
                else if (StoreDetailEquipmentMembership.Identifiers.Contains(orderDetail.Product.Id))
                {
                    //make sure the application does not already exist
                    StoreDetailEquipmentMembership equipmentApplication = GetAll<StoreDetailEquipmentMembership>(true).FirstOrDefault(equipment => equipment.OrderDetailId == orderDetail.Id);
                    if (equipmentApplication == null)
                    {
                        added = true;
                        equipmentApplication = new StoreDetailEquipmentMembership();
                        Add<StoreDetailEquipmentMembership>(equipmentApplication);
                        application = equipmentApplication;
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
        /// UpdateTaxAndShipping
        /// </summary>
        /// <param name="order"></param>
        public void UpdateTaxAndShipping(StoreOrder order)
        {
            StoreAddress address = null;
             
            if (order != null && order.Company != null && order.Company.Addresses != null && order.Company.Addresses.Count > 0)
            {
                address = order.Company.GetCompanyShippingAddress();
            }

            if (address != null && order != null && order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                //finding IsSubscription is set for any OrderDetail Product
                bool shouldBeAnnualized = false;
                order.Total = 0m;
                order.AnnualizedTotal = 0m;
                decimal tax= 0m;
                foreach (StoreOrderDetail orderDetail in order.OrderDetails)
                {
                    if (orderDetail.Product != null && orderDetail.Product.IsSubscription && orderDetail.Product.SubscriptionFrequency == "M")
                        shouldBeAnnualized = true;

                    //look up the address information
                    //set the default values
                    tax = 0m;
                    orderDetail.ShippingCost = 0m;
                   
                    //Retrieve Shipping cost and HasTax values to calculate tax 
                    //calculate the taxes, membership application fee is non-taxable
                    if (orderDetail.Product != null)
                    {
                        decimal costForTax = (orderDetail.Cost * orderDetail.Quantity);
                        if (orderDetail.Coupon != null)
                        {
                            costForTax = costForTax - orderDetail.Coupon.ProductDiscount;
                            orderDetail.DiscountAmount = orderDetail.Coupon.AppFeeDiscount + orderDetail.Coupon.ProductDiscount;
                        }

                        //tax calculated based on full amount except shipping
                        if (orderDetail.Product.HasTax)
                            tax = CalculateTaxes(address, costForTax + (orderDetail.Product.IsMembership() ? 0 : orderDetail.ApplicationCost));

                        if (string.IsNullOrEmpty(orderDetail.ShippingMethod))
                        {
                            orderDetail.ShippingCost = GetShippingCost(orderDetail.Product, address.Country, orderDetail.Quantity);
                        }
                        else
                        {
							//specturm has a supplement option which may impact shipping
                            bool isGiftSupplement = false;
                            if (orderDetail.Product.Id == 39)
                            {
                                StoreDetailCatalog catalogDetails = this.GetAll<StoreDetailCatalog>(false).SingleOrDefault(detail => detail.OrderDetailId == orderDetail.Id);
                                if (catalogDetails != null && catalogDetails.SupplementId == 24)
                                    isGiftSupplement = true;
                            }

                            if (!isGiftSupplement) orderDetail.ShippingCost = GetShippingCost(orderDetail.Product, address.Country, orderDetail.Quantity, orderDetail.ShippingMethod);
                            else orderDetail.ShippingCost = GetShippingCost(orderDetail.Product, address.Country, orderDetail.Quantity, orderDetail.ShippingMethod, true, 0.06m);
                        }
                    }

                    orderDetail.TaxCost = tax;
                    //this is the cost of what to pay now
                    order.Total += orderDetail.Cost * orderDetail.Quantity + orderDetail.TaxCost - orderDetail.DiscountAmount + orderDetail.ShippingCost + orderDetail.ApplicationCost;
                    if (order.Total < 0.0m) order.Total = 0;

                    order.AnnualizedTotal += order.Total;

                    //This is to calculate annualized cost
                    if (shouldBeAnnualized)
                    {
                        tax = 0;
                        if (orderDetail.Product.HasTax)
                        {
                            tax = CalculateTaxes(address, orderDetail.Cost * orderDetail.Quantity) * 11;
                        }

                        order.AnnualizedTotal += orderDetail.Cost * orderDetail.Quantity * 11 + tax + orderDetail.ShippingCost * 11;
                    }

                    if (order.AnnualizedTotal < 0.0m) 
                        order.AnnualizedTotal = 0;
                }
            }
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
                TaxRate taxRateRecord = this.GetAll<TaxRate>().SingleOrDefault(taxRecord => taxRecord.State == address.State && taxRecord.Zip == null);
                if (taxRateRecord != null)
                {
                    taxRate = taxRateRecord.Rate;
                }
                
                if( !string.IsNullOrEmpty(address.Zip) )
                {
                    int zipCode = 0;
                    int.TryParse(address.Zip, out zipCode);
                    if (zipCode > 0)
                    {
                        //look for a state/zip record
                        taxRateRecord = this.GetAll<TaxRate>().SingleOrDefault(taxRecord => taxRecord.State == address.State && taxRecord.Zip == zipCode);
                        if (taxRateRecord != null) taxRate += taxRateRecord.Rate;
                    }
                }
                if (taxRate > 0) tax = (amount.Value * taxRate) / 100m;
            }
            tax = Math.Round(tax, 2);
            return tax;
        }

        /// <summary>
        /// Provide the Product Shipping cost
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        private decimal GetShippingCost(ContextProduct product, string country, int quantity = 1, string shippingMethod = "UPSGround", bool isGiftSupplement = false, decimal? weight = null)
        {
            decimal cost = 0m;
            if (product == null || country == null) throw new Exception("Invalid call to the GetShippingCost method");
            if (product.HasShipping)
            {
                if (product.Weight != null)
                {
                    if (!isGiftSupplement) weight = product.Weight;
                    else if (weight != null) weight += product.Weight;
                }

                if (weight != null && weight.HasValue && !string.IsNullOrEmpty(product.Origin))
                {
                    if (shippingMethod == null) throw new Exception("You need to pass a shipping method for this product");
                    LookProductShippingRate productShippingrate = this.GetAll<LookProductShippingRate>()
                        .FirstOrDefault(rate => rate.Country == country && rate.Origin == product.Origin && rate.ShippingMethod == shippingMethod);
                    if (productShippingrate == null) throw new Exception("We could not find a valid option for the GetShippingCost");
                    cost = productShippingrate.BaseAmount + (quantity * weight.Value * productShippingrate.AmountOrPercent);
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
    }
}
