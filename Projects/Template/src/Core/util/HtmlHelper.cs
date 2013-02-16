using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.util
{
    public class HtmlHelper
    {
        /// <summary>
        /// Get a list of attributes for the EditorTemplates
        /// </summary>
        /// <param name="ViewData"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetAttributes(ViewDataDictionary<dynamic> ViewData, string className)
        {
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            string classValue = (string.IsNullOrWhiteSpace(className) ? string.Empty : className);
            if (!string.IsNullOrWhiteSpace(ViewData["class"] as string)) classValue += " " + ViewData["class"];
            attributes.Add("class", classValue);
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Watermark) && !attributes.ContainsKey("placeholder"))
            {
                attributes.Add("placeholder", ViewData.ModelMetadata.Watermark);
            }
            //todo iterate through html- attributes to add new ones
            return attributes;
        }
    }
}
