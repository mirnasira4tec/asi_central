﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public int PublicationIssueId { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired")]
        [MaxLength(50, ErrorMessageResourceName = "NameLength")]
        public string Name { get; set; }
        public virtual IList<Publication> Publications { get; set; }
    }
}
