﻿using System;
using System.Collections.Generic;
using asi.asicentral.model;
using asi.asicentral.model.store;

namespace asi.asicentral.web.Models
{
    public class CompanyInfoModel
    {
        public CompanyInformation CompanyInfo { get; set; }
        public string Message { get; set; }
        public CompanyStatusCode Status { get; set; }
        public StoreCompany StoreCompany;
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