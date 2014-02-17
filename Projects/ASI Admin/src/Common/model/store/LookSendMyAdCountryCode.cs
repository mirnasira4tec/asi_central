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

        public DateTime CreateDateUTC { get; set; }

        public DateTime UpdateDateUTC { get; set; }

        public string UpdateSource { get; set; }
    }
}
