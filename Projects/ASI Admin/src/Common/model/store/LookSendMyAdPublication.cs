using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookSendMyAdPublication
    {

        public int Id { get; set; }

        public virtual LookMagazineIssue MagazineIssue { get; set; }

        public int? PublicationId { get; set; }

        public string IssueCode { get; set; }
    }
}
