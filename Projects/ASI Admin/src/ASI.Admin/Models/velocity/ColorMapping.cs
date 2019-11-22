using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.web.Models.velocity
{
    public class ColorMapping
    {
        public int CompayId { get; set; }
        public string SupplierColor { get; set; }
        public string ColorGroup { get; set; }
        public string Status { get; set; }
    }
    public class ResultantColorMapping
    {
        public string Status { get; set; }
        public List<ColorMapping> ColorMappings { get; set; }
    }
}
