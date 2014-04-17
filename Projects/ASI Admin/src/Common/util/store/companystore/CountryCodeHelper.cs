using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util.store.companystore
{

    public static class CountryCodeHelper
    {

        public static string NumberCode(this IEnumerable<LookSendMyAdCountryCode> source, string countryName)
        {
            string code = null;
            if (!string.IsNullOrWhiteSpace(countryName))
            {
                code = source
                    .Where(item => item.CountryCodeExist(countryName))
                    .Select(item1 => item1.NumberCode).FirstOrDefault();
            }
            return code;
        }

        public static string Alpha3Code(this IEnumerable<LookSendMyAdCountryCode> source, string countryName)
        {
            string code = null;
            if (!string.IsNullOrWhiteSpace(countryName))
            {
                code = source
                    .Where(item => item.CountryCodeExist(countryName))
                    .Select(item1 => item1.Alpha3).FirstOrDefault();
            }
            return code;
        }

        public static bool IsUSAAddress(this IEnumerable<LookSendMyAdCountryCode> source, string countryName)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(countryName))
                {
                    LookSendMyAdCountryCode code = source
                        .FirstOrDefault(item => item.CountryCodeExist(countryName));
                    if (code != null) result = code.Alpha3.ToUpper() == "USA";
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
