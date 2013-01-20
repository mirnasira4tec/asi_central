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

        [Display(ResourceType = typeof(Resource), Name = "PublicationId")]
        public int PublicationId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PublicationName", Prompt = "PublicationPrompt")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameLength")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "StartDate")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "EndDate")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Description", Prompt = "DescriptionPrompt")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsPublic")]
        public bool IsPublic { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Color")]
        public string Color { get; set; }

        public virtual IList<PublicationIssue> Issues { get; set; }

        public override string ToString()
        {
            return string.Format("Publication: {0} - {1}", PublicationId, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            Publication publication = obj as Publication;
            if (publication != null) equals = publication.PublicationId == PublicationId;
            return equals;
        }

        public override int GetHashCode()
        {
            return PublicationId.GetHashCode();
        }
    }
}
