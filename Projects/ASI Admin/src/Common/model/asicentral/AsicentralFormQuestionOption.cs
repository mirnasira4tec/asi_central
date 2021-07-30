using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
   public class AsicentralFormQuestionOption
    {
        public int Id { get; set; }
        public int FormQuestionId { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string AdditionalData { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        virtual public AsicentralFormQuestion FormQuestion { get; set; }
    }
}
