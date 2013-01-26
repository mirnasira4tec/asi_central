﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.sgr
{
    public class Product
    {
        public Product()
        {
            if (this.GetType() == typeof(Product))
            {
                Categories = new List<Category>();
            }
        }

        [Display(ResourceType = typeof(Resource), Name = "ProductID")]
        [Required]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductName")]
        [Required]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductModelNumber")]
        [Required]
        public string ModelNumber { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductPrice")]
        public Decimal? Price { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductPriceCeiling")]
        public Decimal? PriceCeiling { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductMinOrderQuantity")]
        public string MinimumOrderQuantity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductPaymentTerms")]
        public string PaymentTerms { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resource), Name = "ProductKeySpecifications")]
        public string KeySpecifications { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductSmallImg")]
        public string ImageSmall { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductLargeImg")]
        public string ImageLarge { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductIsActive")]
        public bool IsActive { get; set; }

        [Required]
        public virtual Company Company { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public void CopyTo(Product product)
        {
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
        }
        
        public override string ToString()
        {
            return string.Format("Product: {0} - {1}", Id, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            Product product = obj as Product;
            if (product != null) equals = product.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}