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
        public string pacakage { get; set; }
        public string contract { get; set; }
        public string creditStatus { get; set; }
        public string ecommerce { get; set; }
        public string asiSmartBooksEval { get; set; }
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

    public class CompanyUserCollection {
       public List<CompanyInfoModel> companyInfoList { get; set; }
       public List<UserInfoModel> userInfoList { get; set; }
    }

}