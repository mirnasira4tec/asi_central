using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show.form
{
    public class SHW_FormQuestionOption
    {
        public int Id { get; set; }
        public int FormQuestionId { get; set; }
        public int Sequence { get; set; }
        public string OptionDescription { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        virtual public SHW_FormQuestion FormQuestion { get; set; }
    }
}
