using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model
{
    public class Mail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return "Mail To:" + To + " - Subject:" + Subject;
        }
    }
}
