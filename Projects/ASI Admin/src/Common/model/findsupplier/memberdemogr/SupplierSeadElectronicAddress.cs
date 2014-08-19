using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.findsupplier
{
    public class SupplierSeadElectronicAddress
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int EATPId { get; set; }
        public string ElectronicAddress { get; set; }
        public bool IsPrimary { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MaxSize { get; set; }
        public string Setting { get; set; }
        public string Protocal { get; set; }
        public string Software { get; set; }
    }
}
