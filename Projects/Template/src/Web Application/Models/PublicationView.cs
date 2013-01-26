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
        public PublicationView()
        {
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

        public static PublicationView CreateFromPublication(Publication publication)
        {
            PublicationView view = new PublicationView();
            publication.CopyTo(view);
            return view;
        }

        public Publication GetPublication()
        {
            Publication publication = new Publication();
            this.CopyTo(publication);
            return publication;
        }
    }
}