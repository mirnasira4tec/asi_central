using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using asi.asicentral.model.sgr;

namespace asi.asicentral.web.Models.sgr
{
    public class ViewProduct : Product
    {
        public int CategoryID { set; get; }

        static public ViewProduct CreateFromProduct(Product product)
        {
            ViewProduct viewProduct = new ViewProduct();
            product.CopyTo(viewProduct);
            return viewProduct;
        }

        public Product GetProduct()
        {
            Product product = new Product();
            this.CopyTo(product);
            return product;
        }
    }
}