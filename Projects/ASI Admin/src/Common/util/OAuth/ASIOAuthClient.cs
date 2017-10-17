using asi.asicentral.model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using ASI.EntityModel;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using System.ComponentModel;
using ASI.Services.Http.SmartLink;
using ASI.Contracts.Messages.UserMgmt;
using ASI.Services.Http.Security;
using System.Security.Claims;
using ASI.Services.Messaging;
using System.Net;
using System.Web;

namespace asi.asicentral.oauth
{
    public enum StatusCode
    {
        //API which provide codes for status from MMS
        //http://stage-asiservice.asicentral.com/nanny/api/lookup?typecodename=signonstatus

        //Code list from personify
        [Description("Active")]
        ACTV,
        [Description("Active")]
        ACTIVE,
        [Description("ASI Central")]
        ASICENTRAL,
        //Consider dist_newlie as Active; they are active customer within the first  90 days of membership.
        [Description("Distributor Newlie")]
        DIST_NEWLIE,
        [Description("Delisted")]
        DELS,
        [Description("Delisted")]
        DELISTED,
        [Description("Terminated")]
        TRMN,
        [Description("Terminated")]
        TERMINATED,
        [Description("Corporate Lead")]
        CORP_LEAD,
        [Description("Cross Referenced")]
        CROSS_REFERENCED,
        [Description("Desk Filed")]
        DESKFILED,
        [Description("End_Buyer")]
        END_BUYER,
        [Description("Expired Trial Member")]
        EXPIRED_TRIAL,
        [Description("Inactive")]
        INACTIVE,
        [Description("Lead")]
        LEAD,
        [Description("MMS Datafeed")]
        MMS_LOAD,
        [Description("Non Member")]
        NON_MEMBER,
        [Description("Out Of Business")]
        OUT_OF_BUSINESS,
        [Description("Trial")]
        TRIAL,
    }

    public enum UsageCode
    {
        GNRL,
        EBIL,
        ESHP
    }

    public enum ApplicationCodes
    {
        ASCT, //For ASI Central
        EMES, //For Email-express
        WESP, //For ESP Web
        ASED, //new code for education
        UPSIDE, //For upsidelms
        ASST // For ASI Store
    }
    
    public class ASIOAuthClient
    {
        public static bool IsValidAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) return false;
            var authenticatedUser = GetAuthenticatedUser(accessToken);
            return (authenticatedUser != null);
        }

        public static AuthenticatedUser GetAuthenticatedUser(string accessToken)
        {
            AuthenticatedUser user = null;
            try
            {
                IList<Claim> claims = new List<Claim>();
                var claimsPrincipal = Token.Validate(accessToken, out claims);
                if( claimsPrincipal != null)
                    user = claimsPrincipal.Identity as AuthenticatedUser;
            }
            catch (Exception ex)
            {
                var log = LogService.GetLog(typeof(ASIOAuthClient));
                log.Error(string.Format("GetAuthenticatedUser - exception: {0}", ex.Message));
            }

            return user;
        }

        public static model.User GetUser(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            model.User user = null;
            try
            {
                var authenticatedUser = GetAuthenticatedUser(token);

                if (authenticatedUser != null)
                {
                    int SSOId = Convert.ToInt32(authenticatedUser.UserId);
                    user = GetUser(SSOId);
                    if (user != null)
                    {
                        user.AccessToken = token;
                    }
                    //user.RefreshToken = redirectParams.RefreshToken;
                }
            }
            catch (Exception ex)
            {
                LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                log.Error(string.Format("GetUser - exception: {0}", ex.Message));
            }
            return user;
        }

        public static asi.asicentral.model.User GetUser(int sso)
        {
            model.User user = null;
            ASI.EntityModel.User entityUser = GetEntityUser(sso);
            if(entityUser != null) user = MapEntityModelUserToASIUser(entityUser);
            return user;
        }

        private static ASI.EntityModel.User GetEntityUser(int sso)
        {
            ASI.EntityModel.User user = null;
            try
            {
                //Arrange
                var userRequest = new UserRequest() { UserRequestType = UserRequestType.Retrieve, UserSearchCriteria = new UserSearchCriteria() { Id = sso } };
                //Act
                userRequest.TalkAndWait<UserRequest, UserResponse>(userResponse =>
                {
                    if (userResponse.Users != null && userResponse.Users.Count > 0) user = userResponse.Users.FirstOrDefault();
                });
            }
            catch (Exception ex)
            {
                LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                log.Error(ex.Message);
            }
            return user;
        }

        public static IDictionary<string, string> RefreshToken(string refreshToken, string appCode = null, string appVersion = null)
        {
            ILogService log = LogService.GetLog(typeof(ASIOAuthClient));
            log.Debug("RefreshToken - Start");
            IDictionary<string, string> tokens = null;
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
                var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
                var relativePath = ConfigurationManager.AppSettings["RelativePath"];
                var host = ConfigurationManager.AppSettings["SecurityHost"];
                if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret)
                    && !string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(relativePath))
                {
                    try
                    {
                        if (HttpContext.Current == null || (HttpContext.Current.Request != null && HttpContext.Current.Request.IsLocal))
                        {
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; // ignor Certificate for testing
                        }

                        log.Debug("RefreshToken - Get Client");
                        OAuth2Client oAuth2Client = new OAuth2Client(host, relativePath: relativePath);

                        var hostAddress = HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.UserHostAddress;

                        var oauth2Response = oAuth2Client.Refresh(asiOAuthClientId, asiOAuthClientSecret, refreshToken, appCode, appVersion, userHostAddress: hostAddress).Result;
                        if (oauth2Response != null)
                        {
                            log.Debug("Login_FetchUserDetails - Login - AccessToken " + oauth2Response.AccessToken);
                            log.Debug("Login_FetchUserDetails - Login - RefreshToken " + oauth2Response.RefreshToken);
                            tokens = new Dictionary<string, string>();

                            tokens.Add("AuthToken", oauth2Response.AccessToken);
                            tokens.Add("RefreshToken", oauth2Response.RefreshToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
            }
            log.Debug("RefreshToken - End");
            return tokens;
        }

        //When ever you call this API, you need to reset the ssoid in the cookie using
        public static IDictionary<string, string> IsValidUser(string userName, string password)
        {
            var tokens = Login_FetchUserDetails(userName, password);
            if(tokens != null && !string.IsNullOrEmpty(tokens["AuthToken"]))
            {
                var authenticatedUser = GetAuthenticatedUser(tokens["AuthToken"]);
                return (authenticatedUser != null) ? tokens : null;
            }
            return null;
        }

        public static IDictionary<string, string> Login_FetchUserDetails(string userName, string password)
        {
			ILogService log = LogService.GetLog(typeof(ASIOAuthClient));
			log.Debug("Login_FetchUserDetails - Start");
			IDictionary<string, string> tokens = null;
            var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
            var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
            var relativePath = ConfigurationManager.AppSettings["RelativePath"];
            var host = ConfigurationManager.AppSettings["SecurityHost"];
            if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret) &&
                !string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(relativePath))
            {
                if (HttpContext.Current == null || (HttpContext.Current.Request != null && HttpContext.Current.Request.IsLocal))
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; // ignor Certificate for testing
                }


				log.Debug("Login_FetchUserDetails - ServerCertificateValidationCallback created");
				try
                {
					log.Debug("Login_FetchUserDetails - Login");
                    OAuth2Client oauth2Client = new OAuth2Client(host, relativePath: relativePath);

                    var hostAddress = HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.UserHostAddress;


                    var oauth2Response = oauth2Client.Login(asiOAuthClientId, asiOAuthClientSecret, userName, password, scope: "AsiNumberOptional", userHostAddress: hostAddress).Result;
                    if (oauth2Response != null)
                    {
                        log.Debug("Login_FetchUserDetails - Login - AccessToken " + oauth2Response.AccessToken);
                        log.Debug("Login_FetchUserDetails - Login - RefreshToken " + oauth2Response.RefreshToken);
                        tokens = new Dictionary<string, string>();
						
						tokens.Add("AuthToken", oauth2Response.AccessToken);
                        tokens.Add("RefreshToken", oauth2Response.RefreshToken);
                     }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
				log.Debug("Login_FetchUserDetails - End");
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
                    var user = GetUserByEmail(email);
                    isValidUser = (user != null && user.Email.ToLower() == email);
                }
                catch (Exception ex)
                {
                    isValidUser = false;
                    LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                    log.Error(ex.Message);
                }
            }
            return isValidUser;
        }

        public static model.User GetUserByEmail(string email)
        {
            model.User user = null;
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    //Arrange
                    var userRequest = new UserRequest() { UserRequestType = UserRequestType.Retrieve, UserSearchCriteria = new UserSearchCriteria() { Email = email } };

                    //Act
                    userRequest.TalkAndWait<UserRequest, UserResponse>(responseMessage =>
                    {
                        if (responseMessage.Users != null && responseMessage.Users.Count > 0)
                        {
                            var entityUsers = responseMessage.Users.Where(u => u.StatusCode == StatusCode.ACTV.ToString()).ToList();

                            if (entityUsers != null && entityUsers.Count > 0)
                            {
                                ASI.EntityModel.User entityUser = FilterUserWithEmail(entityUsers, email);
                                if (entityUser != null)
                                {
                                    user = MapEntityModelUserToASIUser(entityUser);
                                }
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                    log.Error(ex.Message);
                }
            }
            return user;
        }

        public static string CreateUser(asi.asicentral.model.User user)
        {
            string ssoId = string.Empty;
            if (user != null)
            {
                try
                {
                    ASI.EntityModel.User entityUser = null;
                    var skipValidateCompany = false;
                    if (user.CompanyId == 0)
                    {
                        var usePersonifyServices = ConfigurationManager.AppSettings["UsePersonifyServices"];
                        if (!string.IsNullOrEmpty(usePersonifyServices) && Convert.ToBoolean(usePersonifyServices))
                        {
                            PersonifyService personifyService = new PersonifyService();
                            try
                            {
                                var company = personifyService.AddCompany(user);
                                skipValidateCompany = company != null && !string.IsNullOrEmpty(company.MasterCustomerId);
                            }
                            catch (Exception ex)
                            {
                                LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                                log.Error(ex.Message);
                                ssoId = ex.Message;
                            }
                        }
                    }
                    entityUser = MapASIUserToEntityModelUser(user, entityUser, true);
                    //ARRANGE
                    var userRequest = new UserRequest() { UserRequestType = UserRequestType.Create, AuditTrail = new AuditTrail() { LoggedInUserId = 1 }, User = entityUser, SkipCompanyValidation = skipValidateCompany };

                    //ACT
                    var message = string.Empty;
                    userRequest.TalkAndWait<UserRequest, UserResponse>(responseMessage =>
                    {
                        ssoId = (responseMessage != null && responseMessage.Users != null && responseMessage.Users.Count > 0 && responseMessage.Users[0] != null && responseMessage.Users[0].Id > 0) ? responseMessage.Users[0].Id.ToString() 
                            : (responseMessage != null && !responseMessage.IsSuccess && responseMessage.Error != null ? responseMessage.Error.ErrorMessage : null);
                    });
                }
                catch (Exception ex)
                {
                    LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                    log.Error(ex.Message);
                    ssoId = ex.Message;
                }
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
                            MemberTypeId = company.MemberTypeNumber,
                            MemberStatus_CD = company.MemberStatus
	                    };
                    }
                }
                catch (Exception ex)
                {
                    LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                    log.Error(ex.Message);
                }
            }
            return user;
        }

        public static bool UpdateUser(asi.asicentral.model.User user, bool isPasswordReset = false)
        {
            bool isUserUpdated = false;
            if (user != null)
            {
                try
                {
                    ASI.EntityModel.User entityUser = null;
                    ASI.EntityModel.Company entityCompany = null;
                    entityCompany = MapASIUserCompanyToEntityModelCompany(user, entityCompany, false);
                    entityUser = MapASIUserToEntityModelUser(user, entityUser, isCreate: false, isPasswordReset: isPasswordReset);

                    var userRequest = new UserRequest() { UserRequestType = UserRequestType.Update, AuditTrail = new AuditTrail() { LoggedInUserId = 1 }, User = entityUser };
                    userRequest.TalkAndWait<UserRequest, UserResponse>(updResponseMessage =>
                    {
                        isUserUpdated = (updResponseMessage != null && updResponseMessage.Users != null && updResponseMessage.Users.Count > 0 && updResponseMessage.Users.FirstOrDefault() != null) ? true : false;
                    });
                }
                catch (Exception ex)
                {
                    isUserUpdated = false;
                    LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                    log.Error(ex.Message);
                }
            }
            return isUserUpdated;
        }

        public static bool ChangePassword(int ssoid, asi.asicentral.model.Security security, bool passwordResetRequired = false)
        {
            bool isPasswordChanged = false;
            if (ssoid != 0 && security != null)
            {
                try
                {
                    asicentral.model.User user = GetUser(ssoid);
                    user.Password = security.Password;
                    user.PasswordAnswer = security.Password;
                    user.TelephonePassword = security.Password;
                    user.PasswordResetRequired = passwordResetRequired;
                    isPasswordChanged = UpdateUser(user, true);
                }
                catch (Exception ex)
                {
                    isPasswordChanged = false;
                    LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                    log.Error(ex.Message);
                }
            }
            return isPasswordChanged;
        }

        public static string GetUserName(string firstName, string lastName)
        {
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(firstName))
            {
                userName = firstName + ' ' + lastName;
                userName = userName.Replace(',', ' ');
                userName = userName.Trim();
            }

            if(string.IsNullOrEmpty(userName))
            {
                LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                log.Error("Username is empty");
            }
            return userName;
        }

        public static bool IsActiveUser(string memberStatus)
        {
            if (!string.IsNullOrEmpty(memberStatus) 
                && (memberStatus == StatusCode.ACTIVE.ToString()
                || memberStatus == StatusCode.ACTV.ToString() 
                || memberStatus == StatusCode.DIST_NEWLIE.ToString()))
                return true;
            else return false;
        }

        private static ASI.EntityModel.User FilterUserWithEmail(IList<ASI.EntityModel.User> entityUsers, string email)
        {
            if (entityUsers != null && entityUsers.Count > 0)
            {
                foreach (ASI.EntityModel.User entityUser in entityUsers)
                {
                    foreach (ASI.EntityModel.Email emailFromDB in entityUser.Emails)
                    {
                        if (!string.IsNullOrEmpty(emailFromDB.Address) && string.Compare(emailFromDB.Address.ToLower(), email.ToLower()) == 0)
                            return entityUser;
                    }
                }
            }
            return null;
        }

        private static ASI.EntityModel.User MapASIUserToEntityModelUser(asi.asicentral.model.User user, ASI.EntityModel.User entityUser, bool isCreate, bool isPasswordReset = false)
        {
            if (user != null)
            {
                if (entityUser == null && user.SSOId != 0) 
                    entityUser = GetEntityUser(user.SSOId);

                if( entityUser == null)
                    entityUser = new ASI.EntityModel.User();
                            
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

                if(user.PasswordResetRequired) entityUser.PasswordResetRequired = "Y";
                else entityUser.PasswordResetRequired = "N";
                
                if (isCreate)
                {
                    entityUser.UserName = user.Email;
                    entityUser.StatusCode = StatusCode.ACTV.ToString();
                }
                if (isCreate || isPasswordReset)
                {
                    entityUser.Password = user.Password;
                    entityUser.PasswordHint = user.Password;
                    entityUser.PasswordResetRequired = "N";
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
                    address.IsPrimary = true;
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
                if ((entityUser.Phones == null || (entityUser.Phones != null && entityUser.Phones.Count == 0)) && (!string.IsNullOrEmpty(user.Phone) || !string.IsNullOrEmpty(user.Fax)))
                {
                    if(entityUser.Phones == null) entityUser.Phones = new List<Phone>();
                    if (!string.IsNullOrEmpty(user.Phone))
                    {
                        phone = new Phone();
                        phone.IsPrimary = true;
                        phone.UserId = user.SSOId;
                        entityUser.Phones.Add(phone);
                    }
                    if (!string.IsNullOrEmpty(user.Fax))
                    {
                        fax = new Phone();
                        fax.IsFax = true;
                        fax.UserId = user.SSOId;
                        entityUser.Phones.Add(fax);
                    }
                }
                else if (entityUser.Phones != null && entityUser.Phones.Count > 0)
                {
                    phone = entityUser.Phones.Where(ph => ph.IsPrimary).FirstOrDefault();
                    if (string.IsNullOrEmpty(user.Phone)) { entityUser.Phones.Remove(phone); phone = null; }
                    fax = entityUser.Phones.Where(ph => ph.IsFax).FirstOrDefault();
                    if (string.IsNullOrEmpty(user.Fax)) { entityUser.Phones.Remove(fax); fax = null; }
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

                if (!string.IsNullOrEmpty(company.Type))
                {
                    string membertype = company.Type;
                    membertype = user.MemberType_CD;
                }
                else company.Type = user.MemberType_CD;
                
                contact.Title = user.Title;
                contact.Suffix = user.Suffix;
                
                ASI.EntityModel.Address address = null;
                if (company.Addresses != null && company.Addresses.Count > 0)
                {
                    address = company.Addresses.Where(add => add.IsPrimary).SingleOrDefault();
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

        private static asi.asicentral.model.User MapEntityModelUserToASIUser(ASI.EntityModel.User entityUser)
        {
            model.User user = null;
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
                    if (!string.IsNullOrEmpty(entityUser.PasswordResetRequired) && entityUser.PasswordResetRequired == "Y")
                        user.PasswordResetRequired = true;
                    else user.PasswordResetRequired = false;
                    user.LastName = entityUser.LastName;
                    user.Prefix = entityUser.Prefix;
                    user.Suffix = entityUser.Suffix;
                    user.CompanyId = entityUser.CompanyId;
                    user.StatusCode = entityUser.StatusCode;
                    if (entityUser.Types != null && entityUser.Types.Count > 0)
                        user.MemberType_CD = entityUser.Types.ElementAt(0);
                    if (entityUser.Phones != null && entityUser.Phones.Count > 0)
                    {
                        Phone phone = entityUser.Phones.FirstOrDefault(ph => ph.IsPrimary);
                        if (phone != null)
                        {
                            user.Phone = phone.PhoneNumber;
                            user.PhoneAreaCode = phone.AreaCode;
                        }
						Phone fax = entityUser.Phones.FirstOrDefault(ph => ph.IsFax);
                        if (fax != null)
                        {
                            user.Fax = fax.PhoneNumber;
                            user.FaxAreaCode = fax.AreaCode;
                        }
                    }

                    if (entityUser.Addresses != null && entityUser.Addresses.Count > 0)
                    {
						Address address = entityUser.Addresses.FirstOrDefault(add => add.UsageCode == UsageCode.GNRL.ToString());
                        if (address != null)
                        {
                            user.Street1 = address.AddressLine1;
                            user.Street2 = address.AddressLine2;
                            if (!string.IsNullOrEmpty(address.State)) user.State = address.State.Trim();
                            if (!string.IsNullOrEmpty(address.CountryCode)) user.CountryCode = address.CountryCode.Trim();
                            if (!string.IsNullOrEmpty(address.County)) user.Country = address.County.Trim();
                            if (!string.IsNullOrEmpty(address.City)) user.City = address.City.Trim();
                            if (!string.IsNullOrEmpty(address.ZipCode)) user.Zip = address.ZipCode.Trim();
                        }
                    }

                    if (entityUser.CompanyId != 0)
                    {
                        var usePersonifyServices = ConfigurationManager.AppSettings["UsePersonifyServices"];
                        if (!string.IsNullOrEmpty(usePersonifyServices) && Convert.ToBoolean(usePersonifyServices))
                        {
                            IBackendService personify = new PersonifyService();
                            CompanyInformation companyInfo = personify.GetCompanyInfoByIdentifier(entityUser.CompanyId);
                            if (companyInfo != null)
                            {
                                user.CompanyName = companyInfo.Name;
                                user.CompanyId = companyInfo.CompanyId;
                                user.MemberType_CD = companyInfo.MemberType;
                                user.MemberStatus_CD = companyInfo.MemberStatus;
								user.MemberTypeId = companyInfo.MemberTypeNumber;
	                            user.AsiNumber = companyInfo.ASINumber;
                                user.ExternalReference = string.Join(";", companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
                                user.CompanyPhone = companyInfo.Phone;
								//Fill details from personify, in case UMS not provided below details
                                if(string.IsNullOrEmpty(user.City))
                                {
								    if (!string.IsNullOrEmpty(companyInfo.Street1))
								    {
									    user.Street1 = companyInfo.Street1;
									    user.Street2 = companyInfo.Street2;
								    }
								    if (!string.IsNullOrEmpty(companyInfo.City)) user.City = companyInfo.City;
								    if (!string.IsNullOrEmpty(companyInfo.State)) user.State = companyInfo.State;
								    if (!string.IsNullOrEmpty(companyInfo.Zip)) user.Zip = companyInfo.Zip;
								    if (!string.IsNullOrEmpty(companyInfo.Country)) user.Country = companyInfo.Country;
                                }
                            }
                        }
                        else
                        {
                            var companyRequest = new CompanyRequest() { CompanySearchCriteria = new CompanySearchCriteria() { Id = entityUser.CompanyId } };
                            companyRequest.TalkAndWait<CompanyRequest, CompanyResponse>(companyResponse =>
                            {
                                if (companyResponse.Companies != null && companyResponse.Companies.Count > 0)
                                {
                                    var company = companyResponse.Companies.FirstOrDefault();
                                    user.CompanyName = company.Name;
                                    user.CompanyId = company.Id;
                                    user.AsiNumber = company.AsiNumber;
                                    user.MemberType_CD = company.Type;
                                    if (company.Contacts != null && company.Contacts.Count > 0)
                                    {
                                        Contact contact = company.Contacts.ElementAt(0);
                                        user.Title = contact.Title;
                                        user.Suffix = contact.Suffix;
                                    }
                                }
                            });
                        }
                    }
                }
                if (!IsActiveUser(user.MemberStatus_CD)) user.AsiNumber = null;
            }
            catch (Exception ex)
            {
                LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                log.Error(ex.Message);
            }
            return user;
        }

        public static string CreateUserWithOutCompany(asi.asicentral.model.User user)
        {
            string ssoId = string.Empty;
            if (user != null)
            {
                try
                {
                    ASI.EntityModel.User entityUser = null;
                    entityUser = MapASIUserToEntityModelUser(user, entityUser, true);
                    //ARRANGE
                    var userRequest = new UserRequest() { UserRequestType = UserRequestType.Create, AuditTrail = new AuditTrail() { LoggedInUserId = 1 }, User = entityUser, SkipCompanyValidation = true };
                    //ACT
                    var message = string.Empty;
                    userRequest.TalkAndWait<UserRequest, UserResponse>(responseMessage =>
                    {
                        ssoId = (responseMessage != null && responseMessage.Users != null && responseMessage.Users.Count > 0 && responseMessage.Users[0] != null && responseMessage.Users[0].Id > 0) ? responseMessage.Users[0].Id.ToString()
                            : (responseMessage != null && !responseMessage.IsSuccess && responseMessage.Error != null ? responseMessage.Error.ErrorMessage : null);
                    });
                }
                catch (Exception ex)
                {
                    LogService log = LogService.GetLog(typeof(ASIOAuthClient));
                    log.Error(ex.Message);
                    ssoId = ex.Message;
                }
            }
            return ssoId;
        }
    }
}

