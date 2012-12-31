using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model
{
    public class Publication
    {
        public Publication()
        {
            if (this.GetType() == typeof(Publication))
                Issues = new List<PublicationIssue>();
        }

        public int PublicationId { get; set; }

        [Required(ErrorMessageResourceName="FieldRequired")]
        [MaxLength(50,ErrorMessageResourceName="NameLength")]
        public string Name { get; set; }
        public virtual IList<PublicationIssue> Issues { get; set; }
    }
}
