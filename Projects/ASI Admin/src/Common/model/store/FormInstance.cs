using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class FormInstance
    {
        public FormInstance()
        {
            if (this.GetType() == typeof(FormInstance))
            {
                Values = new List<FormValue>();
            }
        }

        public int Id { get; set; }
        public FormType FormType { get; set; }
        public int FormTypeId { get; set; }
        public string Email { get; set; }
        public string Salutation { get; set; }
        public string ExternalReference { get; set; }
        public virtual IList<FormValue> Values { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
