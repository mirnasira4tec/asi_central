using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class OrderAddOnOption
    {
        public OrderAddOnOption()
        {
        }        
        public OrderAddOnOption(string name, decimal price, string subscripiton, string memberType = "Supplier")
        {
            Name = name;
            Price = price;
            Subscripiton = subscripiton;
            MemberType = memberType;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Subscripiton { get; set; }
        public string MemberType { get; set; }

    }
}
