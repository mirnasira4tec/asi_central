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
        [Required]
        public string Name { get; set; }

        public virtual IList<Company> Companies { get; set; }
        public virtual IList<Product> Products { get; set; }

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
