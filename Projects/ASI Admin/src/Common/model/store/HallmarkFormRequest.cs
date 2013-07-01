using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class HallmarkFormRequest
    {
        public int OrderDetailId { get; set; }
        public string WebRequest { get; set; }
        public string WebResponse { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
