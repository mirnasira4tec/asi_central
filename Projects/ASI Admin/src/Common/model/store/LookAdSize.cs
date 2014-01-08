using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookAdSize
    {

        public int Id { get; set; }

        public LookMagazineIssue Issue { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }
}
