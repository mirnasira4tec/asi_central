using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowProfileRequestOptionalDetails
    {
        public int Id { get; set; }
        public int ProfileRequestId { get; set; }
        public int ProfileOptionalDataLabelId { get; set; }
        public string UpdateValue { get; set; }
        public string OrigValue { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual ShowProfileRequests ProfileRequests { get; set; }
        public virtual ShowProfileOptionalDataLabel ProfileOptionalDataLabel { get; set; }
    }
}
