using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show.form
{
    public class SHW_ShowFormInstance
    {
        public int Id { get; set; }
        public int FormInstanceId { get; set; }
        public int ShowId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        virtual public SHW_FormInstance FormInstance { get; set; }
        virtual public ShowASI Show { get; set; }
    }
}
