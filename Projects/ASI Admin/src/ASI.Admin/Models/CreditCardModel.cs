using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model
{
    public class CreditCardModel
    {
        public CreditCardModel()
        {
            SuccessMessages = new List<string>();
            ErrorMessages = new List<string>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }
        public IList<string> SuccessMessages { get; set; }
        public IList<string> ErrorMessages { get; set; }
    }
}