﻿using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using asi.asicentral.model.show;
using System.Web.Mvc;

namespace asi.asicentral.web.Models.Show
{
    public class ShowModel
    {
        public int Id { get; set; }

        public const String TAB_SHOWTYPE = "showType";
        public const String TAB_SHOWYEAR = "ShowYear";
        public String ShowTab { get; set; }
        public int? ShowTypeId { get; set; }

       
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string Name { get; set; }

        
        //[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        //[StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        //public string Type { get; set; }

       
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

         [DataType(DataType.Date)]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime EndDate { get; set; }

         public IList<SelectListItem> ShowType { get; set; }

    }
}