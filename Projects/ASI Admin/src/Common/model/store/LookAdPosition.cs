using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookAdPosition
    {

        public int Id { get; set; }

        public LookMagazineIssue Issue { get; set; }

        public string Description { get; set; }
    }
}
