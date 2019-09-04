using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class CatalogContactSale
    {
        public int CatalogContactSaleId { get; set; }
        public string ASINumber { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public string ASIRep { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public string OtherOptions { get; set; }
        public bool ArtworkInFile { get; set; }
        public virtual ICollection<CatalogContactSaleDetail> CatalogContactSaleDetails { get; set; }
        public virtual ICollection<CatalogArtWorks> CatalogArtWorks { get; set; }
    }
}
