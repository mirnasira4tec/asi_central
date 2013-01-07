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

        [Display(Name = "PublicationId", ResourceType = typeof(Resource))]
        public int PublicationId { get; set; }

        [Display(Name = "PublicationName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(50, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        public virtual IList<PublicationIssue> Issues { get; set; }
    }
}
