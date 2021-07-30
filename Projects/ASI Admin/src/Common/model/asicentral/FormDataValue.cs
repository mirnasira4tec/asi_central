using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class FormDataValue
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int InstanceId { get; set; }
        public string Value { get; set; }
        public string UpdateValue { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public AsicentralFormInstance FormInstance { get; set; }
        public AsicentralFormQuestion Question { get; set; }

    }
}
