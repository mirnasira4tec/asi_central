using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class CatalogArtWorks
    {
        public int ArtworkId { get; set; }
        public string MediaLink { get; set; }
        public string AsiNo { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
    }
}
