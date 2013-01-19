using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.sgr
{
    public class Product
    {
        [Display(Name = "Id")]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        public string ModelNumber { get; set; }

        public Decimal Price { get; set; }

        public Decimal? PriceCeiling { get; set; }

        public string MinimumOrderQuantity { get; set; }

        public string PaymentTerms { get; set; }

        [DataType(DataType.MultilineText)]
        public string KeySpecifications { get; set; }

        public string ImageSmall { get; set; }

        public string ImageLarge { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        public virtual Company Company { get; set; }

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
