using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model
{
    public class PublicationIssue
    {
        public PublicationIssue()
        {
            if (this.GetType() == typeof(PublicationIssue))
                Publications = new List<Publication>();
        }

        [Display(Name = "PublicationIssueId", ResourceType = typeof(Resource))]
        public int PublicationIssueId { get; set; }

        [Display(Name = "PublicationIssueName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(50, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }
        public virtual ICollection<Publication> Publications { get; set; }

        public override string ToString()
        {
            return string.Format("PublicationIssue: {0} - {1}", PublicationIssueId, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            PublicationIssue publication = obj as PublicationIssue;
            if (publication != null) equals = publication.PublicationIssueId == PublicationIssueId;
            return equals;
        }

        public override int GetHashCode()
        {
            return PublicationIssueId.GetHashCode();
        }
    }
}
