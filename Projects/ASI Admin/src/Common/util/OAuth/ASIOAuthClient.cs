using asi.asicentral.model;
using DotNetOpenAuth.AspNet;
using ASI.Jade.OAuth2;
using ASI.Jade.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using ASI.Jade.Utilities;
using ASI.Jade.v2;
using ASI.EntityModel;
using System.Threading.Tasks;
using asi.asicentral.services;
using asi.asicentral.interfaces;

namespace asi.asicentral.oauth
{
    public enum StatusCode
    {
        ACTV
    }

    public enum UsageCode
    {
        GNRL,
        EBIL,
        ESHP
    }

    public enum MemberType
    {
        DIST,
        SPLR,
        DECR,
        MLRP,
        EDBY,
        AFFL,
        UNKN
    }

    public enum MemberTypeDesc
    {
        Supplier,
        Distributor,
        MultiLineRep,
        Decorator,
        Guest,
    }

    public enum ApplicationCodes
    {
        ASIC, //For ASI Central
        EMES, //For Email-express
        WESP //For ESP Web
    }
    
    public class ASIOAuthClient
    {
        private static ASI.Jade.UserManagement.User _juser { get; set; }
        private static ASI.Jade.UserManagement.User JUser
        {
            get
            {
                if (_juser == null)
                {
                    _juser = new ASI.Jade.UserManagement.User();
                    return _juser;
                }
                else { return _juser; }
            }
        }

        public static asi.asicentral.model.User GetUser(string token)
        {
            asi.asicentral.model.User user = null;
            try
            {
                ASI.Jade.Utilities.CrossApplication.RedirectParams redirectParams = CrossApplication.ParseTokenUrl(token);
                IDictionary<string, string> userDetails = null;
                var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
                var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
                if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
                {
                    ASI.Jade.OAuth2.WebServerClient webServerClient = new WebServerClient(asiOAuthClientId, asiOAuthClientSecret);
                    userDetails = webServerClient.GetUserDetails(redirectParams.AccessToken);
                }
                if (userDetails != null && userDetails.Count > 0)
                {
                    int SSOId = Convert.ToInt32(userDetails["sign_in_id"]);
                    user = GetUser(SSOId);
                    if (redirectParams != null)
                    {
                        user.AccessToken = redirectParams.AccessToken;
                        user.RefreshToken = redirectParams.RefreshToken;
                    }
                }
            }
            catch 
            {
                return null;
            }
            return user;
        }

        public static asi.asicentral.model.User GetUser(int sso)
        {
            model.User user = null;
            try
            {
                ASI.EntityModel.User entityUser = Task.Factory.StartNew(() => UMS.UserSearch(sso).Result, TaskCreationOptions.LongRunning).Result;
                user = MapEntityModelUserToASIUser(entityUser, user);
            }
            catch (System.Exception Ex)
            {
                string Message = Ex.Message;
            }
            return user;
        }

        public static IDictionary<string, string> RefreshToken(string refreshToken)
        {
            IDictionary<string, string> tokens = null;
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
                var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
                if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
                {
                    ASI.Jade.OAuth2.WebServerClient webServerClient = new WebServerClient(asiOAuthClientId, asiOAuthClientSecret);
                    try
                    {
                        tokens = webServerClient.RefreshToken(refreshToken);
                    }
                    catch { }
                }
            }
            return tokens;
        }

        //When ever you call this API, you need to reset the ssoid in the cookie using
        public static IDictionary<string, string> IsValidUser(string userName, string password)
        {
            IDictionary<string, string> tokens = null;
            var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
            var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
            if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
            {
                ASI.Jade.OAuth2.WebServerClient webServerClient = new WebServerClient(asiOAuthClientId, asiOAuthClientSecret);
                try
                {
                    tokens = webServerClient.Login(userName, password);
                }
                catch { }
            }
            return tokens;
        }

        public static IDictionary<string, string> Login_FetchUserDetails(string userName, string password)
        {
            IDictionary<string, string> tokens = null;
            var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
            var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
            if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
            {
                ASI.Jade.OAuth2.WebServerClient webServerClient = new WebServerClient(asiOAuthClientId, asiOAuthClientSecret);
                try
                {
                    tokens = webServerClient.Login(userName, password);
                    if (tokens != null && tokens.Count > 0)
                    {
                        string accessToken = string.Empty;
                        string refreshToken = string.Empty;
                        if (tokens.ContainsKey("AuthToken")) accessToken = tokens["AuthToken"];
                        if (tokens.ContainsKey("RefreshToken")) refreshToken = tokens["RefreshToken"];
                        tokens = null;
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            tokens = null;
                            tokens = webServerClient.GetUserDetails(accessToken);
                            if (tokens != null && tokens.Count > 0)
                            {
                                tokens.Add("AuthToken", accessToken);
                                tokens.Add("RefreshToken", refreshToken);
                            }

                        }
                    }
                }
                catch { }
            }
            return tokens;
        }

        public static bool IsValidEmail(string email)
        {
            bool isValidUser = false;
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    List<ASI.EntityModel.User> entityUsers = Task.Factory.StartNew(() => UMS.UserSearch(new UMS.UserSearchCriteria { EMail = email }).Result, TaskCreationOptions.LongRunning).Result;
                    if (entityUsers != null 
                        && entityUsers.Count > 0 
                        && entityUsers.Where(usr => usr.Emails != null 
                                            && usr.Emails.Count > 0
                                            && usr.Emails.Where(mail => mail.Address == email).ToList() != null
                                            ) != null)
                    {
                        isValidUser = true;
                    }
                }
                catch { isValidUser = false; }
            }
            return isValidUser;
        }

        public static asi.asicentral.model.User GetUserByEmail(string email)
        {
            model.User user = null;
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    List<ASI.EntityModel.User> entityUser = Task.Factory.StartNew(() => UMS.UserSearch(new UMS.UserSearchCriteria { EMail = email }).Result, TaskCreationOptions.LongRunning).Result;
                    if (entityUser != null && entityUser.Count > 0 && entityUser.Where(usr => usr.Emails != null && usr.Emails.Count > 0
                        && usr.Emails.Where(mail => mail.Address == email).ToList() != null
                        ) != null)
                    {
                        user = MapEntityModelUserToASIUser(entityUser.ElementAt(0), user);
                        return user;
                    }
                }
                catch { }
            }
            return null;
        }

        public static string CreateUser(asi.asicentral.model.User user)
        {
            string ssoId = string.Empty;
            if (user != null)
            {
                try
                {
                    ASI.EntityModel.User entityUser = null;
                    var usePersonifyServices = ConfigurationManager.AppSettings["UsePersonifyServices"];
                    if (!string.IsNullOrEmpty(usePersonifyServices) && Convert.ToBoolean(usePersonifyServices))
                    {
                        PersonifyService personifyService = new PersonifyService();
                        try
                        {
                            PersonifyDataASI.SaveCustomerOutput company = personifyService.AddCompanyByNameAndMemberTypeId(user.CompanyName, user.MemberTypeId);
                            if (company != null && string.IsNullOrEmpty(company.MasterCustomerId))
                                user.CompanyId = Convert.ToInt32(company.MasterCustomerId);
                        }
                        catch { }
                    }
                    else if(user.CompanyId == 0) user.CompanyId = 114945;
                    entityUser = MapASIUserToEntityModelUser(user, entityUser, true);
                    ssoId = Task.Factory.StartNew(() => UMS.UserCreate(entityUser).Result, TaskCreationOptions.LongRunning).Result;
                }
                catch { }
            }
            return ssoId;
        }

        public static asi.asicentral.model.User GetCompanyByASINumber(string asiNumber)
        {
            asi.asicentral.model.User user = null;
            var usePersonifyServices = ConfigurationManager.AppSettings["UsePersonifyServices"];
            if (!string.IsNullOrEmpty(usePersonifyServices) && Convert.ToBoolean(usePersonifyServices) && !string.IsNullOrEmpty(asiNumber))
            {
                try
                {
                    IBackendService personifyService = new PersonifyService();
	                CompanyInformation company = personifyService.GetCompanyInfoByAsiNumber(asiNumber);
                    if (company != null)
                    {
	                    user = new model.User
	                    {
		                    CompanyName = company.Name,
							CompanyId = company.CompanyId,
							AsiNumber = asiNumber,
							MemberType_CD = company.MemberType,
	                    };
                    }
                }
                catch { }
            }
            return user;
        }

        public static bool UpdateUser(asi.asicentral.model.User user)
        {
            bool isUserUpdated = false;
            if (user != null)
            {
                try
                {
                    ASI.EntityModel.User entityUser = null;
                    ASI.EntityModel.Company entityCompany = null;
                    entityCompany = MapASIUserCompanyToEntityModelCompany(user, entityCompany, false);
                    entityUser = MapASIUserToEntityModelUser(user, entityUser, false);
                    var result = Task.Factory.StartNew(() => UMS.UserUpdate(entityUser).Result, TaskCreationOptions.LongRunning).Result;
                    isUserUpdated = true;
                }
                catch { isUserUpdated = false; }
            }
            return isUserUpdated;
        }

        public static bool ChangePassword(int ssoid, asi.asicentral.model.Security security)
        {
            bool isPasswordChanged = false;
            if (ssoid != 0 && security != null)
            {
                try
                {
                    ASI.Jade.UserManagement.DataObjects.Security jadeSecurity = new ASI.Jade.UserManagement.DataObjects.Security();
                    if(MapASISecurityToJadeSecurity(security, jadeSecurity) != null)
                        isPasswordChanged = JUser.ChangeSecurity(ssoid, jadeSecurity);
                }
                catch { isPasswordChanged = false; }
            }
            return isPasswordChanged;
        }

        public static string GetMemberTypeDescription(string memberType)
        {
            MemberType code = (MemberType)Enum.Parse(typeof(MemberType), memberType);        
            switch (code)
            {
                case MemberType.SPLR:
                    return MemberTypeDesc.Supplier.ToString();
                case MemberType.DIST:
                    return MemberTypeDesc.Distributor.ToString();
                case MemberType.MLRP:
                    return MemberTypeDesc.MultiLineRep.ToString();
                case MemberType.DECR:
                    return MemberTypeDesc.Decorator.ToString();
                case MemberType.UNKN:
                default:
                    return MemberTypeDesc.Guest.ToString();
            }
        }

        public static string GetMemberTypeUMSCode(int code)
        {
            switch (code)
            {
                case 1:
                case 6:
                case 16:
                    return MemberType.DIST.ToString();
                case 2:
                case 7:
                    return MemberType.SPLR.ToString();
                case 3:
                case 12:
                    return MemberType.DECR.ToString();
                case 9:
                    return MemberType.EDBY.ToString();
                case 13:
                    return MemberType.AFFL.ToString();
                default:
                    return MemberType.UNKN.ToString();
            }
        }

        private static ASI.EntityModel.User MapASIUserToEntityModelUser(asi.asicentral.model.User user, ASI.EntityModel.User entityUser, bool isCreate)
        {
            if (user != null)
            {
                if (entityUser == null && user.SSOId != 0) entityUser = Task.Factory.StartNew(() => UMS.UserSearch(user.SSOId).Result, TaskCreationOptions.LongRunning).Result;
                else entityUser = new ASI.EntityModel.User();
                            
                if(!string.IsNullOrEmpty(user.Email))
                {
                    Email email = null;
                    if (entityUser.Emails == null)
                    {
                        entityUser.Emails = new List<Email>();
                        email = new Email();
                        entityUser.Emails.Add(email);
                    }
                    else if (entityUser.Emails.Count > 0)
                    {
                        email = entityUser.Emails.ElementAt(0);
                    }
                    if(isCreate) email.IsPrimary = true;
                    email.Address = user.Email;
                }

                if (isCreate)
                {
                    entityUser.UserName = user.Email;
                    entityUser.Password = user.Password;
                    entityUser.PasswordHint = user.Password;
                    entityUser.StatusCode = StatusCode.ACTV.ToString();
                }
                entityUser.FirstName = user.FirstName;
                entityUser.MiddleName = user.MiddleName;
                entityUser.LastName = user.LastName;
                entityUser.Prefix = user.Prefix;
                entityUser.Suffix = user.Suffix;
                entityUser.CompanyId = user.CompanyId;

                Address address = null;
                if (entityUser.Addresses == null)
                {
                    entityUser.Addresses = new List<Address>();
                    address = new Address();
                    address.IsDefault = true;
                    address.UsageCode = UsageCode.GNRL.ToString();
                    entityUser.Addresses.Add(address);
                }
                else if (entityUser.Addresses.Count > 0)
                {
                    address = entityUser.Addresses.ElementAt(0);
                }
               
                address.AddressLine1 = user.Street1;
                address.AddressLine2 = user.Street2;
                address.State = user.State;
                address.CountryCode = user.CountryCode;
                address.County = user.Country;
                address.City = user.City;
                address.State = user.State;
                address.ZipCode = user.Zip;
                
                string memberType = string.Empty;
                if (entityUser.Types == null)
                {
                    entityUser.Types = new List<string>();
                    entityUser.Types.Add(user.MemberType_CD);
                }
                else
                {
                    memberType = entityUser.Types.ElementAt(0);
                    memberType = user.MemberType_CD;
                }

                Phone phone = null;
                Phone fax = null;
                if (entityUser.Phones == null && (!string.IsNullOrEmpty(user.Phone) ||  !string.IsNullOrEmpty(user.Fax)))
                {
                    entityUser.Phones = new List<Phone>();
                    if (!string.IsNullOrEmpty(user.Phone))
                    {
                        phone = new Phone();
                        phone.IsPrimary = true;
                        entityUser.Phones.Add(phone);
                    }
                    if (!string.IsNullOrEmpty(user.Fax))
                    {
                        fax = new Phone();
                        fax.IsFax = true;
                        entityUser.Phones.Add(fax);
                    }
                }
                else if (entityUser.Phones != null && entityUser.Phones.Count > 0)
                {
                    phone = entityUser.Phones.Where(ph => ph.IsPrimary).FirstOrDefault();
                    fax = entityUser.Phones.Where(ph => ph.IsFax).FirstOrDefault();
                }
                if (phone != null)
                {
                    phone.PhoneNumber = user.Phone;
                    phone.AreaCode = user.PhoneAreaCode;

                }
                if (fax != null)
                {
                    fax.PhoneNumber = user.Fax;
                    fax.AreaCode = user.FaxAreaCode;
                }
            }
            return entityUser;
        }

        private static ASI.EntityModel.Company MapASIUserCompanyToEntityModelCompany(asi.asicentral.model.User user, ASI.EntityModel.Company company, bool isCreate)
        {
            if (user != null)
            {
                if (company == null) company = new ASI.EntityModel.Company();
                company.Name = user.CompanyName;
                company.Id = user.CompanyId;
                ASI.EntityModel.Contact contact = null;
                if (company.Contacts != null && company.Contacts.Count > 0)
                {
                    contact = company.Contacts.ElementAt(0);
                }
                else
                {
                    company.Contacts = new List<Contact>();
                    contact = new ASI.EntityModel.Contact();
                    company.Contacts.Add(contact);
                }

                if (company.Types != null && company.Types.Count > 0)
                {
                    string membertype = company.Types.ElementAt(0);
                    membertype = user.MemberType_CD;
                }
                else
                {
                    company.Types = new List<string>();
                    company.Types.Add(user.MemberType_CD);
                }

                contact.Title = user.Title;
                contact.Suffix = user.Suffix;
                
                ASI.EntityModel.Address address = null;
                if (company.Addresses != null && company.Addresses.Count > 0)
                {
                    address = company.Addresses.Where(add => add.IsDefault).SingleOrDefault();
                }
                else
                {
                    company.Addresses = new List<Address>();
                    address = new ASI.EntityModel.Address();
                    company.Addresses.Add(address);
                }

                address.AddressLine1 = user.Street1;
                address.AddressLine2 = user.Street2;
                address.State = user.State;
                address.CountryCode = user.CountryCode;
                address.UsageCode = UsageCode.GNRL.ToString();
                address.County = user.Country;
                address.ZipCode = user.Zip;
                address.City = user.City;
            }
            return company;
        }

        private static ASI.Jade.UserManagement.DataObjects.Security MapASISecurityToJadeSecurity(asi.asicentral.model.Security security, ASI.Jade.UserManagement.DataObjects.Security jadeSecurity)
        {
            if (security != null)
            {
                if (jadeSecurity == null) jadeSecurity = new ASI.Jade.UserManagement.DataObjects.Security();
                jadeSecurity.Password = security.Password;
                jadeSecurity.PasswordAnswer = security.Password;
                jadeSecurity.PasswordHint = security.PasswordHint;
                jadeSecurity.PasswordQuestion = security.PasswordQuestion;
            }
            return jadeSecurity;
        }

        private static asi.asicentral.model.User MapEntityModelUserToASIUser(ASI.EntityModel.User entityUser, asi.asicentral.model.User user)
        {
            try
            {
                if (entityUser != null)
                {
                    user = new model.User();
                    user.SSOId = entityUser.Id;
                    if(entityUser.Emails != null && entityUser.Emails.Count > 0)
                        user.Email = entityUser.Emails.ElementAt(0).Address;
                    user.UserName = entityUser.UserName;
                    user.Password = entityUser.Password;
                    user.FirstName = entityUser.FirstName;
                    user.MiddleName = entityUser.MiddleName;
                    user.LastName = entityUser.LastName;
                    user.Prefix = entityUser.Prefix;
                    user.Suffix = entityUser.Suffix;
                    user.CompanyId = entityUser.CompanyId;
                    user.StatusCode = entityUser.StatusCode;
                    if (entityUser.Types != null && entityUser.Types.Count > 0)
                        user.MemberType_CD = entityUser.Types.ElementAt(0);
                    if (entityUser.Phones != null && entityUser.Phones.Count > 0)
                    {
                        Phone phone = entityUser.Phones.Where(ph => ph.IsPrimary).SingleOrDefault();
                        if (phone != null)
                        {
                            user.Phone = phone.PhoneNumber;
                            user.PhoneAreaCode = phone.AreaCode;
                        }
                        Phone fax = entityUser.Phones.Where(ph => ph.IsFax).SingleOrDefault();
                        if (fax != null)
                        {
                            user.Fax = fax.PhoneNumber;
                            user.FaxAreaCode = fax.AreaCode;
                        }
                    }

                    if (entityUser.Addresses != null && entityUser.Addresses.Count > 0)
                    {
                        Address address = entityUser.Addresses.Where(add => add.UsageCode == UsageCode.GNRL.ToString()).SingleOrDefault();
                        if (address != null)
                        {
                            user.Street1 = address.AddressLine1;
                            user.Street2 = address.AddressLine2;
                            user.State = address.State.Trim();
                            user.CountryCode = address.CountryCode.Trim();
                            user.Country = address.County.Trim();
                            user.City = address.City.Trim();
                            user.Zip = address.ZipCode.Trim();
                        }
                    }

                    if (entityUser.CompanyId != 0)
                    {
                        ASI.EntityModel.Company entityCompany = ASI.Jade.Company.Retriever.Get(entityUser.CompanyId);
                        if (entityCompany != null)
                        {
                            user.CompanyName = entityCompany.Name;
                            user.CompanyId = entityCompany.Id;
                            user.AsiNumber = entityCompany.AsiNumber;
                            if (entityCompany.Contacts != null && entityCompany.Contacts.Count > 0)
                            {
                                ASI.EntityModel.Contact contact = entityCompany.Contacts.ElementAt(0);
                                user.Title = contact.Title;
                                user.Suffix = contact.Suffix;
                            }
                            if (entityCompany.Types != null && entityCompany.Types.Count > 0)
                                user.MemberType_CD = entityCompany.Types.ElementAt(0);
                        }
                    }
                }
            }
            catch { }
            return user;
        }
    }
}
