using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class ResearchData
    {
        public int Id { get; set; }
        public int ResearchImportId { get; set; }
        public string  ImageUrl{ get; set; }
        public string Year { get; set; }
        public string  Product { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string Industry { get; set; }
        public string Gender { get; set; }
        public string Generation { get; set; }
        public string Topic { get; set; }
        public string Others { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }

        //[ForeignKey("ResearchImportId")]
        public virtual ResearchImport ResearchImport { get; set; }
    }
}
