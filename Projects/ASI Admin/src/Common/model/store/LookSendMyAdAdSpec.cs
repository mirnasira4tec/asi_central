using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookSendMyAdAdSpec
    {

        public int Id { get; set; }

        public LookAdSize AdSize { get; set; }
        
        public int? AdSpecId { get; set; }
    }
}
