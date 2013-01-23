using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Models
{
    /// <summary>
    /// Created to wrap around Publication and includes list for the drop down
    /// </summary>
    public class PublicationView : Publication
    {
        private Publication _publication;

        public override int PublicationId
        {
            get
            {
                return _publication.PublicationId;
            }
            set
            {
                _publication.PublicationId = value;
            }
        }

        public override string Name
        {
            get
            {
                return _publication.Name;
            }
            set
            {
                _publication.Name = value;
            }
        }

        public override string Description
        {
            get
            {
                return _publication.Description;
            }
            set
            {
                _publication.Description = value;
            }
        }

        public override DateTime StartDate
        {
            get
            {
                return _publication.StartDate;
            }
            set
            {
                _publication.StartDate = value;
            }
        }

        public override DateTime? EndDate
        {
            get
            {
                return _publication.EndDate;
            }
            set
            {
                _publication.EndDate = value;
            }
        }

        public override bool IsPublic
        {
            get
            {
                return _publication.IsPublic;
            }
            set
            {
                _publication.IsPublic = value;
            }
        }

        public override IList<PublicationIssue> Issues
        {
            get
            {
                return _publication.Issues;
            }
            set
            {
                _publication.Issues = value;
            }
        }

        public override string Color
        {
            get
            {
                return _publication.Color;
            }
            set
            {
                _publication.Color = value;
            }
        }

        public SelectList ColorList
        {
            get
            {
                IList<SelectListItem> colors = new List<SelectListItem>();
                colors.Add(new SelectListItem() { Text = "Blue", Value = "Blue", Selected = false });
                colors.Add(new SelectListItem() { Text = "Green", Value = "Green", Selected = false });
                colors.Add(new SelectListItem() { Text = "Yellow", Value = "Yellow", Selected = false });
                return new SelectList(colors, "Value", "Text");
            }
        }

        public PublicationView()
        {
            _publication = new Publication();
        }

        public static PublicationView GetPublicationView(Publication publication)
        {
            PublicationView publicationView = new PublicationView();
            publicationView._publication = publication;
            return publicationView;
        }

        public Publication GetPublication()
        {
            return _publication;
        }

        public override bool Equals(object obj)
        {
            return _publication.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _publication.GetHashCode();
        }

        public override string ToString()
        {
            return _publication.ToString();
        }
    }
}