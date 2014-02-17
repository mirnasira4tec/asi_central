using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookSendMyAdPublication : IDateUTCAndSource
    {

        public int Id { get; set; }

        public virtual LookMagazineIssue MagazineIssue { get; set; }

        public int PublicationId { get; set; }

        public string IssueCode { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateSource { get; set; }
    }
}
