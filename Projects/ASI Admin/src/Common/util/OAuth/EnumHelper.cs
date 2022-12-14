using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util
{
    public static class EnumHelper
    {

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static string GetDesciption<T>(string enumValue)
        {
            var code = (Enum)Enum.Parse(typeof(T), enumValue);
            var attribute = asi.asicentral.util.EnumHelper.GetAttributeOfType<System.Attribute>(code);
            if (attribute != null)
                return ((DescriptionAttribute)(attribute)).Description;
            else return null;
        }
    }
}
