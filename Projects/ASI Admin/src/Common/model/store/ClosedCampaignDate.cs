using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class ClosedCampaignDate
    {

        public virtual int ID { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual bool Reactivated { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual string CreateSource { get; set; }

        public virtual DateTime? Updatedate { get; set; }

        public virtual string UpdateSource { get; set; }
    }
}
