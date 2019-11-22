using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.velocity
{
    public class ColorMapData
    {
        [Display(Name = "Company Id")]
        [Required(ErrorMessage="Company id is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Company Id")]
        public int CompanyId { get; set; }

        [Display(Name = "Color Data")]
        [Required(ErrorMessage="Colors are required")]
        public string ColorData { get; set; }
    }
}