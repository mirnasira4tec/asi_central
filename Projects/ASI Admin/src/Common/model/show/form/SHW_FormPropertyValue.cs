using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show.form
{
    public class SHW_FormPropertyValue
    {
        public int Id { get; set; }
        public int FormInstanceId { get; set; }
        public int FormQuestionId { get; set; }        
        public string OrigValue { get; set; }
        public string RevisionValue { get; set; }
        public string Label { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        virtual public SHW_FormInstance FormInstance { get; set; }
        virtual public SHW_FormQuestion FormQuestion { get; set; }
    }
}
