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
        public virtual int PublicationId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PublicationName", Prompt = "PublicationPrompt")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NameLength")]
        public virtual string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "StartDate", Prompt = "DateTimePrompt")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.DateTime)]
        public virtual DateTime StartDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "EndDate", Prompt = "DatePrompt")]
        [DataType(DataType.Date)]
        public virtual DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Description", Prompt = "DescriptionPrompt")]
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsPublic")]
        public virtual bool IsPublic { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Color")]
        public virtual string Color { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Number")]
        public int? Number { get; set; }

        public virtual IList<PublicationIssue> Issues { get; set; }

        public override string ToString()
        {
            return string.Format("Publication: {0} - {1}", PublicationId, Name);
        }

        public void CopyTo(Publication publication)
        {
            if (publication == null) throw new Exception("Cannot copy data to a null object");
            publication.PublicationId = PublicationId;
            publication.Color = Color;
            publication.Description = Description;
            publication.EndDate = EndDate;
            publication.IsPublic = IsPublic;
            publication.Name = Name;
            publication.StartDate = StartDate;
            //might need to make copies of issues. Not required for now
            foreach (PublicationIssue issue in this.Issues)
            {
                PublicationIssue original = publication.Issues.Where(iss => iss.PublicationIssueId == issue.PublicationIssueId).FirstOrDefault();
                if (original != null) issue.CopyTo(original);
                else publication.Issues.Add(issue);
            }
            for (int i = publication.Issues.Count - 1; i > -0; i--)
            {
                PublicationIssue original = publication.Issues.ElementAt(i);
                PublicationIssue newOne = this.Issues.Where(iss => iss.PublicationIssueId == original.PublicationIssueId).FirstOrDefault();
                if (newOne == null) publication.Issues.Remove(original);
            }

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
