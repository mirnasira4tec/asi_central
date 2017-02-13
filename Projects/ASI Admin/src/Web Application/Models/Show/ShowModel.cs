using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using asi.asicentral.model.show;
using System.Web.Mvc;

namespace asi.asicentral.web.models.show
{
    public class ShowModel : PagerModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public const String TAB_SHOWTYPE = "showType";
        public String ShowTab { get; set; }
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? ShowTypeId { get; set; }
        public int? year { get; set; }
       
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string Name { get; set; }

        public string Type { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(500, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime EndDate { get; set; }

        public IList<ShowASI> Show { set; get; }
        public IList<SelectListItem> ShowType { get; set; }
    }
}