using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show.form
{
    public class SHW_FormType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NotificationEmails { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        virtual public List<SHW_FormQuestion> FormQuestions { get; set; }
    }
}
