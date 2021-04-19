using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class AsicentralFormValue
    {
        // labels for CC fields
        public static readonly string CC_HOLDER_NAME = "CC Holder Name";
        public static readonly string CC_TYPE = "CC Type";
        public static readonly string CC_NUMBER = "CC Number";
        public static readonly string CC_EXP_MONTH = "CC Exp Month";
        public static readonly string CC_EXP_YEAR = "CC Exp Year";
        public static readonly string CC_TOKEN_ID = "CC TokenId";
        public static readonly string CC_AUTH_REFERENCE = "CC AuthReference";
        public static readonly string CC_ADDRESS = "CC Address";
        public static readonly string CC_CITY = "CC City";
        public static readonly string CC_STATE = "CC State";
        public static readonly string CC_POSTALCODE = "CC Postal Code";
        public static readonly string CC_COUNTRY = "CC Country";
        public static readonly string IPADDRESS = "IPAddress";
        public static readonly string COMPANY_NAME = "Company Name";
        public static readonly string CC_REQUEST_TOKEN = "CC Request Token";
        public static readonly string CC_RESPONSE_CODE = "CC Response Code";
        public static readonly string CC_RESPONSE_MESSAGE = "CC Response Message";
        public static readonly string CC_AVS_RESULT = "CC AVS Result";
        public static readonly string CC_COMPANY = "CC Company";
        public static readonly string CC_FIRST_NAME = "CC First Name";
        public static readonly string CC_LAST_NAME = "CC Last Name";

        public static readonly string[] CC_Lables = { CC_HOLDER_NAME, CC_TYPE, CC_NUMBER, CC_EXP_MONTH,
            CC_EXP_YEAR, CC_TOKEN_ID, CC_AUTH_REFERENCE, CC_ADDRESS, CC_CITY, CC_STATE,
            CC_POSTALCODE, CC_COUNTRY, CC_REQUEST_TOKEN, CC_RESPONSE_CODE, CC_RESPONSE_MESSAGE, 
            CC_AVS_RESULT, CC_COMPANY, CC_FIRST_NAME, CC_LAST_NAME };

        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int InstanceId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual AsicentralFormInstance Instance { get; set; }
    }
}
