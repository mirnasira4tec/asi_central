using DotNetOpenAuth.AspNet;
using ASI.Jade.OAuth2;
using ASI.Jade.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using ASI.Jade.Utilities;
using ASI.Jade.UserManagement.DataObjects;

namespace asi.asicentral.oauth
{
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
                ASI.Jade.UserManagement.DataObjects.User jadeuser = JUser.Get(sso);
                user = MapEntityModelUserToASIUser(jadeuser, user);
            }
            catch { }
            return user;
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
                    tokens = webServerClient.Login_FetchUserDetails("", userName, password);
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
                    List<ASI.Jade.UserManagement.DataObjects.User> jadeusers = JUser.Get(0,25,null, null, email, null, null);
                    if (jadeusers != null && jadeusers.Count > 0 && jadeusers.Where(user => user.Email == email) != null) isValidUser = true;
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
                    List<ASI.Jade.UserManagement.DataObjects.User> jadeusers = JUser.Get(0, 25, null, null, email, null, null);
                    if (jadeusers != null && jadeusers.Count > 0 && jadeusers.Where(usr => usr.Email == email) != null)
                    {
                        MapEntityModelUserToASIUser(jadeusers.ElementAt(0), user);
                        return user;
                    }
                }
                catch { }
            }
            return null;
        }

        public static string CreateUser(asi.asicentral.model.User user)
        {
            string userDetails = string.Empty;
            if (user != null)
            {
                try
                {
                    ASI.Jade.UserManagement.DataObjects.User jadeUser = new ASI.Jade.UserManagement.DataObjects.User();
                    userDetails = JUser.Create(jadeUser);
                }
                catch {  }
            }
            return userDetails;
        }

        public static bool UpdateUser(string ssoid, asi.asicentral.model.User user)
        {
            bool isUserUpdated = false;
            if (!string.IsNullOrEmpty(ssoid) && user != null)
            {
                try
                {
                    ASI.Jade.UserManagement.DataObjects.User jadeUser = new ASI.Jade.UserManagement.DataObjects.User();
                    JUser.Update(ssoid, jadeUser);
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

        private static ASI.Jade.UserManagement.DataObjects.User MapASIUserToEntityModelUser(asi.asicentral.model.User user, ASI.Jade.UserManagement.DataObjects.User jadeuser, bool isCreate)
        {
            if (user != null)
            {
                if (jadeuser == null) jadeuser = new ASI.Jade.UserManagement.DataObjects.User();
                jadeuser.SSOId = user.SSOId;
                jadeuser.Email = user.Email;
                jadeuser.UserName = user.UserName;
                jadeuser.InternalUserId = user.InternalUserId;
                jadeuser.IndividualId = user.IndividualId;
                jadeuser.Password = user.Password;
                jadeuser.FirstName = user.FirstName;
                jadeuser.MiddleName = user.MiddleName;
                jadeuser.LastName = user.LastName;
                jadeuser.Prefix = user.Prefix;
                jadeuser.Suffix = user.Suffix;
                if (isCreate)
                {
                    jadeuser.CreateDate = user.CreateDate;
                    jadeuser.PasswordHint = user.PasswordHint;
                    jadeuser.PasswordAnswer = user.PasswordAnswer;
                    jadeuser.PasswordQuestionCode = user.PasswordQuestionCode;
                    jadeuser.PasswordQuestion = user.PasswordQuestion;
                }

                jadeuser.UpdateDate = user.UpdateDate;
                jadeuser.UpdateSource = user.UpdateSource;
                jadeuser.CompanyId = user.CompanyId;
                
                jadeuser.IsTelephoneUpdatesAllowed = user.IsTelephoneUpdatesAllowed;
                jadeuser.TelephonePassword = user.TelephonePassword;
                jadeuser.PasswordResetRequired = user.PasswordResetRequired;
                jadeuser.PasswordResetKey = user.PasswordResetKey;
                jadeuser.PasswordResetExpireDate = user.PasswordResetExpireDate;
                jadeuser.TerminatedDate = user.TerminatedDate;
                jadeuser.IsSalesRep = user.IsSalesRep;
                jadeuser.IsPVAdmin = user.IsPVAdmin;
                jadeuser.IsConnectPrimary = user.IsConnectPrimary;
                jadeuser.StatusCode = user.StatusCode;
                jadeuser.SignonTypeCode = user.SignonTypeCode;
                jadeuser.MmsLink = user.MmsLink;
                jadeuser.AsiNumber = user.AsiNumber;
                jadeuser.CompanyName = user.CompanyName;
                jadeuser.MemberStatus_CD = user.MemberStatus_CD;
                jadeuser.MemberType_CD = user.MemberType_CD;
                jadeuser.Phone = user.Phone;
                jadeuser.Cell = user.Cell;
                jadeuser.Fax = user.Fax;
            }
            return jadeuser;
        }

        private static ASI.EntityModel.Company MapASIUserCompanyToEntityModelCompany(asi.asicentral.model.User user, ASI.EntityModel.Company company, bool isCreate)
        {
            if (user != null)
            {
                if (company == null) company = new ASI.EntityModel.Company();
                ASI.EntityModel.Contact contact = null;
                if (company.Contacts != null && company.Contacts.Count > 0)
                {
                    contact = company.Contacts.ElementAt(0);
                }
                else
                {
                    contact = new ASI.EntityModel.Contact();
                    company.Contacts.Add(contact);
                }
                contact.Title = user.Title;
                contact.Suffix = user.Suffix;

                ASI.EntityModel.Address address = null;
                if (company.Contacts.ElementAt(0).Addresses != null &&
                    company.Contacts.ElementAt(0).Addresses.Count > 0)
                {
                    address = company.Contacts.ElementAt(0).Addresses.ElementAt(0);
                }
                else
                {
                    address = new ASI.EntityModel.Address();
                    contact.Addresses.Add(address);
                }

                address.AddressLine1 = user.Street1;
                address.AddressLine2 = user.Street2;
                address.State = user.State;
                address.CountryCode = user.CountryCode;
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

        private static asi.asicentral.model.User MapEntityModelUserToASIUser(ASI.Jade.UserManagement.DataObjects.User jadeuser, asi.asicentral.model.User user)
        {
            try
            {
                if (jadeuser != null)
                {
                    user = new model.User();
                    user.SSOId = jadeuser.SSOId;
                    user.Email = jadeuser.Email;
                    user.UserName = jadeuser.UserName;
                    user.InternalUserId = jadeuser.InternalUserId;
                    user.IndividualId = jadeuser.IndividualId;
                    user.Password = jadeuser.Password;
                    user.FirstName = jadeuser.FirstName;
                    user.MiddleName = jadeuser.MiddleName;
                    user.LastName = jadeuser.LastName;
                    user.Prefix = jadeuser.Prefix;
                    user.Suffix = jadeuser.Suffix;
                    user.CreateDate = jadeuser.CreateDate;
                    user.UpdateDate = jadeuser.UpdateDate;
                    user.UpdateSource = jadeuser.UpdateSource;
                    user.CompanyId = jadeuser.CompanyId;
                    user.PasswordHint = jadeuser.PasswordHint;
                    user.PasswordAnswer = jadeuser.PasswordAnswer;
                    user.PasswordQuestionCode = jadeuser.PasswordQuestionCode;
                    user.PasswordQuestion = jadeuser.PasswordQuestion;
                    user.IsTelephoneUpdatesAllowed = jadeuser.IsTelephoneUpdatesAllowed;
                    user.TelephonePassword = jadeuser.TelephonePassword;
                    user.PasswordResetRequired = jadeuser.PasswordResetRequired;
                    user.PasswordResetKey = jadeuser.PasswordResetKey;
                    user.PasswordResetExpireDate = jadeuser.PasswordResetExpireDate;
                    user.TerminatedDate = jadeuser.TerminatedDate;
                    user.IsSalesRep = jadeuser.IsSalesRep;
                    user.IsPVAdmin = jadeuser.IsPVAdmin;
                    user.IsConnectPrimary = jadeuser.IsConnectPrimary;
                    user.StatusCode = jadeuser.StatusCode;
                    user.SignonTypeCode = jadeuser.SignonTypeCode;
                    user.MmsLink = jadeuser.MmsLink;
                    user.AsiNumber = jadeuser.AsiNumber;
                    user.CompanyName = jadeuser.CompanyName;
                    user.MemberStatus_CD = jadeuser.MemberStatus_CD;
                    user.MemberType_CD = jadeuser.MemberType_CD;
                    user.Phone = jadeuser.Phone;
                    user.Cell = jadeuser.Cell;
                    user.Fax = jadeuser.Fax;

                    if (jadeuser.CompanyId != 0)
                    {
                        ASI.EntityModel.Company jadeCompany = ASI.Jade.Company.Retriever.Get(jadeuser.CompanyId);
                        if (jadeCompany != null)
                        {
                            if (jadeCompany.Contacts != null && jadeCompany.Contacts.Count > 0)
                            {
                                ASI.EntityModel.Contact contact = jadeCompany.Contacts.ElementAt(0);
                                user.Title = contact.Title;
                                user.Suffix = contact.Suffix;
                                if (jadeCompany.Contacts.ElementAt(0).Addresses != null &&
                                   jadeCompany.Contacts.ElementAt(0).Addresses.Count > 0)
                                {
                                    ASI.EntityModel.Address address = jadeCompany.Contacts.ElementAt(0).Addresses.ElementAt(0);
                                    user.Street1 = address.AddressLine1 + " " + address.AddressLine2;
                                    user.Street2 = address.AddressLine3 + " " + address.AddressLine4;
                                    user.State = address.State;
                                    user.CountryCode = address.CountryCode;
                                    user.Country = address.County;
                                    user.Zip = address.ZipCode;
                                    user.City = address.City;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return user;
        }
    }
}
