using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class FormValue
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int FormInstanceId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
