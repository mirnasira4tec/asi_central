using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show.form
{
    public class SHW_FormInstance
    {
        public SHW_FormInstance()
        {
            if (this.GetType() == typeof(SHW_FormInstance))
            {
                PropertyValues = new List<SHW_FormPropertyValue>();
            }
        }

        public int Id { get; set; }

        public int FormTypeId { get; set; }

        public string Email { get; set; }

        public string RequestReference { get; set; }

        public string Identity { get; set; }

        public string SenderIP { get; set; }

        public string EmailSentForShow { get; set; }

        public string Status { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual SHW_FormType FormType { get; set; }
        public virtual List<SHW_FormPropertyValue> PropertyValues { get; set; }
        public virtual List<SHW_ShowFormInstance> ShowFormInstances { get; set; }
    }
}
