using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class FormType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Implementation { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
		public virtual IList<FormInstance> FormInstances { get; set; } 
    }
}
