using asi.asicentral.model.sgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRImport.model
{
    public class ProductView : Product
    {
        public string Category { get; set; }

        public Product GetProduct()
        {
            Product product = new Product();
            product.Id = this.Id;
            product.Name = this.Name;
            product.Company = this.Company;
            product.ImageLarge = this.ImageLarge;
            product.ImageSmall = this.ImageSmall;
            product.IsActive = this.IsActive;
            product.KeySpecifications = this.KeySpecifications;
            product.MinimumOrderQuantity = this.MinimumOrderQuantity;
            product.ModelNumber = this.ModelNumber;
            product.PaymentTerms = this.PaymentTerms;
            product.Price = this.Price;
            product.PriceCeiling = this.PriceCeiling;
            return product;
        }
    }
}
