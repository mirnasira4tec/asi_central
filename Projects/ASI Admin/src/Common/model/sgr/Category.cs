using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.sgr
{
    public class Category
    {
        public static int CATEGORY_ALL = 32;

        public Category()
        {
            if (this.GetType() == typeof(Category))
            {
                Companies = new List<Company>();
                Products = new List<Product>();
            }
        }

        [Display(ResourceType = typeof(Resource), Name = "CategoryID")]
        [Required]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CategoryName")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public void CopyTo(Category category)
        {
            category.Id = Id;
            category.Name = Name;
            category.Products = Products;
        }

        public override string ToString()
        {
            return string.Format("Category: {0} - {1}", Id, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            Category category = obj as Category;
            if (category != null) equals = category.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
