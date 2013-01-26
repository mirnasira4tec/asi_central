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
            this.CopyTo(product);
            return product;
        }
    }
}
