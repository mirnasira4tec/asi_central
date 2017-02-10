using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.excit
{
    public class SupUpdateRequestDetail
    {
        public int Id { get; set; }
        public int SupUpdateRequestId { get; set; }
        public int SupUpdateFieldId { get; set; }
        public string UpdateValue { get; set; }
        public string OrigValue { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual SupUpdateRequest UpdateRequest { get; set; }
        public virtual SupUpdateField UpdateField { get; set; }
    }
}
