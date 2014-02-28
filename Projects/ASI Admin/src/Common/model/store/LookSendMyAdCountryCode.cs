using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookSendMyAdCountryCode : IDateUTCAndSource
    {

        public int Id { get; set; }

        public string CountryName { get; set; }

        public string Alpha2 { get; set; }

        public string Alpha3 { get; set; }

        public string NumberCode { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateSource { get; set; }

        public bool CountryCodeExist(string country)
        {
            bool result = false;
            if(country != null &&
               (this.CountryName.ToLower() == country.ToLower()
               || this.Alpha3.ToLower() == country.ToLower()
               || this.Alpha2.ToLower() == country.ToLower()))
            {
                result = true;
            }
            return result;
        }
    }
}
