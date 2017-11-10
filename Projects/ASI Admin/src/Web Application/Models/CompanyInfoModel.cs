using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models
{
    public class CompanyInfoModel
    {
        public asi.asicentral.model.CompanyInformation companyInfo { get; set; }
        public string message { get; set; }
        public CompanyStatusCode status { get; set; }
    }
    public enum CompanyStatusCode
    {
        Fail,
        Exists,
        Created,
    }
    public class UserInfoModel
    {
        public asi.asicentral.model.User user { get; set; }
        public string message { get; set; }
        public CompanyStatusCode status { get; set; }
    }

}