using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class ResearchImport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }

        virtual public List<ResearchData> ResearchDataList { get; set; }
    }
}
