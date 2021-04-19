using System;
using System.Collections.Generic;
using System.Linq;
using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.services;
using asi.asicentral.services.PersonifyProxy;
using Moq;
using asi.asicentral.util.store;
using PersonifySvcClient;
using asi.asicentral.model.personify;
using asi.asicentral.oauth;
using NUnit.Framework;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class PersonifyTest
    {
        [Test]
        public void PlaceOrderNewCompanyTest()
        {
            IStoreService storeService = MockupStoreService();
            var supplierSpecials = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 77);
            var emailExpress = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 61);
            var order = CreateStoreOrder(string.Empty, new ContextProduct[] { supplierSpecials, emailExpress }, "PlaceOrderNewCompanyTest");
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [Test]
        [Ignore("Ignore a PlaceOrderExistingCompanyTest")]
        public void PlaceOrderExistingCompanyTest()
        {  // supplierSpecials and emailExpress don't have personify integrations any more
            IStoreService storeService = MockupStoreService();
            var supplierSpecials = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 77);
            var emailExpress = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 61);
            var order = CreateStoreOrder("30279", new ContextProduct[] { supplierSpecials, emailExpress }, "PlaceOrderExistingCompanyTest");
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [Test]
        public void GetASICompData()
        {
            var acctid = "000006880201";
            PersonifyClient.GetASICOMPData(acctid);

            acctid = "000010610433";
            PersonifyClient.GetASICOMPData(acctid);

        }



        [Test]
        public void UpdateASICompData()
        {
            var param = new List<string>() { "123456", "000010610432", "0", "PROFITMAKER", "FULL ACCESS", "CURRENT", "Yes", "Yes", "No", "WEB_USER" };
            param = new List<string>() { "", "000010615052", "0", "", "", "", "", "", "Yes", "WEB_ADMIN" };
            PersonifyClient.UpdateASICompData(param);
        }

        [Test]
        public void AddEEXSubscription()
        {
            var tag = DateTime.Now.Ticks.ToString();
            tag = tag.Substring(tag.Length - 5);

            var storeService = MockupStoreService();
            var personifyService = new PersonifyService(storeService);
            var user = new User()
            {
                AsiNumber = "",
                FirstName = "EEX" + tag,
                LastName = "EEXSubscription",
                Email = string.Format("DistEEX{0}@gmail.com", tag),
                PhoneAreaCode = "201",
                Phone = "34" + tag,
                CompanyName = "Distributor EEX Subscription " + tag,
                MemberType_CD = "Distributor",
                Street1 = "1 Street1",
                City = "Trevose",
                State = "PA",
                Country = "USA"
            };

            var companyInfo = personifyService.AddEEXSubscription(user, true);
            Assert.IsNotNull(companyInfo);

            user = new User()
            {
                AsiNumber = "",
                FirstName = "EEX-supplier" + tag,
                LastName = "EEXSubscription",
                Email = string.Format("SupEEX{0}@gmail.com", tag),
                PhoneAreaCode = "202",
                Phone = "34" + tag,
                CompanyName = "Supplier EEX Subscription",
                MemberType_CD = "Supplier",
                Street1 = "1 Street1",
                City = "Trevose",
                State = "PA",
                Country = "USA"
            };

            companyInfo = personifyService.AddEEXSubscription(user, false);
            Assert.IsNotNull(companyInfo);

            user = new User()
            {
                AsiNumber = "",
                FirstName = "EEX-Deco" + tag,
                LastName = "EEXSubscription",
                Email = string.Format("DecoEEX{0}@gmail.com", tag),
                PhoneAreaCode = "203",
                Phone = "34" + tag,
                CompanyName = "Decorator EEX Subscription",
                MemberType_CD = "Decorator",
                Street1 = "1 Street1",
                City = "Trevose",
                State = "PA",
                Country = "USA"
            };

            companyInfo = personifyService.AddEEXSubscription(user, false);
            Assert.IsNotNull(companyInfo);
        }

        public void AsicompPackage()
        {

        }

        [Test]
        [Ignore("EmailOptOut")]
        public void EmailOptOut()
        {
            var email = "testing@111.com";
            var codes = new List<string>() { "OPTOUT_ASI_Marketing", "OPTOUT_Editorial", "OPTOUT_ASI_Show_Mkt", "OPTOUT_Global_ASI_Show", "OPTOUT_GLOBAL_ASI" };
            var output = PersonifyClient.OptOutEmailSubscription(email, codes);

            Assert.IsNotNull(output);
        }

        [Test]
        public void PackageOrderTest()
        {
            IStoreService storeService = MockupStoreService();
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 7);
            var order = CreateStoreOrder("12345", new ContextProduct[] { distributor }, "PlaceBundleOrderTest");
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [Test]
        public void PackageOrderWithCoupon()
        {
            var storeService = MockupStoreService("STANDARD129");
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 7);
            var context = storeService.GetAll<Context>(true).FirstOrDefault(c => c.Id == 8);
            var order = CreateStoreOrder("12345", new ContextProduct[] { distributor }, "PackageOrderDistributorCoupon", "STANDARD129", 129, context);
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [Test]
        public void EmailMrktOrderTest()
        {
            IStoreService storeService = MockupStoreService();
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 126);
            var order = CreateStoreOrder("181255", new ContextProduct[] { distributor }, "EmailMrktOrderTest");
            order.OrderDetails[0].OptionId = 1;
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [Test]
        public void NoOrderCreatedForTerminatedCompany()
        {
            IStoreService storeService = MockupStoreService();
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 2);
            var order = CreateStoreOrder("10275773", new ContextProduct[] { distributor }, "NoOrderCreatedForTerminatedCompany");
            order.Company.Name = "Terminateddist001@gmail.Com";
            order.Company.Phone = "9999990001";
            var companyInfo = CreatePersonifyOrder(order, storeService, MockupPersonifyService(storeService, order, StatusCode.TERMINATED.ToString()));
            Assert.IsNotNull(companyInfo);
            Assert.IsTrue(companyInfo.IsTerminated());
            Assert.IsNull(order.BackendReference);
        }

        [Test]
        public void AddPersonifyCompanyForDualDecorator()
        {
            var storeService = MockupStoreService();
            IBackendService personify = new PersonifyService(storeService);
            var company = GetStoreCompany("NewMembership",
                                          "1511112222",
                                          "newMembership@unittest.com",
                                          "Distributor");

            var companyInfo = GetPersonifyCompany(personify, company);
            personify.UpdateCompanyStatus(company, StatusCode.ACTIVE);
            company.MemberStatus = StatusCode.ACTIVE.ToString();

            var order = new StoreOrder()
            {
                Company = company,
                OrderRequestType = "Distributor",
            };

            var product = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 70);
            order.OrderDetails.Add(new StoreOrderDetail()
            {
                Id = 1,
                ApplicationCost = 0,
                Cost = 0,
                OptionId = 0,
                Quantity = 1,
                Product = product,
            });

            var creditCard = new CreditCard()
            {
                Type = "Visa",
                Number = "4111111111111111",
                ExpirationDate = new DateTime(2018, 11, 15),
                CardHolderName = "ASI Store",
                Address = "Street",
                City = "City",
                State = "NJ",
                Country = "USA",
                PostalCode = "98123",
            };

            var profile = personify.SaveCreditCard(order, creditCard);
            Assert.IsNotNull(profile);
            Assert.AreNotEqual(string.Join(";", companyInfo.MasterCustomerId, companyInfo.SubCustomerId),
                               order.Company.ExternalReference);
            Assert.AreNotEqual("distributor", order.Company.MemberType.ToLower());
        }

        [Test]
        public void AddCompany()
        {
            var tag = DateTime.Now.Ticks;
            var accountInfo = new User
            {
                FirstName = "New First Name",
                LastName = "New Last Name",
                Email = String.Format("user{0}@addCompany.com", tag),
                CompanyName = "New Company1 " + tag,
                Street1 = "4800 Street Rd",
                City = "Trevose",
                State = "PA",
                Zip = "19053",
                Country = "USA",
                MemberTypeId = 6,
                CountryCode = "USA"
            };
            IBackendService personify = new PersonifyService(MockupStoreService());
            var companyInfo = personify.AddCompany(accountInfo);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
            Assert.AreEqual("ASICENTRAL", companyInfo.MemberStatus);
        }

        [Test]
        public void LookupCompany()
        {
            IBackendService personify = new PersonifyService();
            var companyInformation = personify.GetCompanyInfoByAsiNumber("30279");
            Assert.IsNotNull(companyInformation.CompanyId);
            Assert.IsNotNull(companyInformation.City);
            Assert.IsNotNull(companyInformation.MemberType);
            Assert.IsNotNull(companyInformation.MemberStatus);
            companyInformation = personify.GetCompanyInfoByIdentifier(5806901);
            Assert.IsNotNull(companyInformation.Name);
            Assert.IsNotNull(companyInformation.City);
            Assert.IsNotNull(companyInformation.MemberType);
        }

        [Test]
        public void EquipmentASINumberTest()
        {
            IBackendService personify = new PersonifyService();
            string[] asiNumbers = { "18200", "12555" };

            //Equipment ASI Numbers
            //"12310", "12550", "12553", "12555", "12600", 
            //                            "14703", "18200", "14970", "14971", "14972", 
            //                            "14973", "14974", "14976", "14977", "14980", 
            //                            "14981", "16000", "16001", "16002", "16004",
            //                            "16008", "16010", "18200", "18201", "18202",
            //                            "18203", "18205"

            foreach (string number in asiNumbers)
            {
                var companyInformation = personify.GetCompanyInfoByAsiNumber(number);
                Assert.IsNotNull(companyInformation.CompanyId);
                Assert.IsNotNull(companyInformation.City);
                Assert.IsNotNull(companyInformation.MemberType, "EQUIPMENT");
                Assert.IsNotNull(companyInformation.MemberStatus);
            }
        }

        [Test]
        public void AddPhoneNumberTest()
        {
            var companyInfo = PersonifyClient.GetCompanyInfoByASINumber("33020");
            if (companyInfo != null)
            {
                var cusComm = PersonifyClient.AddPhoneNumber("2222222222", "USA", companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
            }
            else
            {
                Assert.IsTrue(false, "Could not find the Company forASI# 33020");
            }
        }

        [Test]
        public void TestSP()
        {
            var result = PersonifyClient.ExecutePersonifySP(PersonifyClient.SP_UPDATE_MMS_EMS_SIGNON, new List<string>() { "000000125724", "0", "240989" });
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetCompanyCreditCards()
        {
            StoreCompany company = new StoreCompany()
            {
                ASINumber = "125724",
                Name = "ASI"
            };
            var creditCards = PersonifyClient.GetCompanyCreditCards(company, "ASI");
            if (creditCards.Any())
            {
                foreach (var creditCard in creditCards)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(creditCard.CardNumber));
                }
            }
            else
            {
                Assert.IsTrue(false, "Could not find any credit cards for ASI# 33020");
            }
        }

        [Test]
        public void CreatCompany()
        {
            var companyName = "Createcompany" + DateTime.Now.Ticks;
            StoreCompany company = GetStoreCompany(companyName, "2122223333", "createCompany@unittest.com", "SUPPLIER");
            IBackendService personify = new PersonifyService(MockupStoreService());
            var companyInfo = personify.CreateCompany(company, "SUPPLIER");
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("SUPPLIER", companyInfo.MemberType);
            Assert.AreEqual(companyName, companyInfo.Name);
            Assert.IsTrue(!string.IsNullOrEmpty(company.ExternalReference));
        }

        [Test]
        public void TestIsNewMembership()
        {
            var storeService = MockupStoreService();

            // test active distributor is buying supplier membership
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 5);
            var order = CreateStoreOrder("10275773", new ContextProduct[] { distributor }, "TestIsNewMembership");

            order.Company = GetStoreCompany("Create Company Dist",
                              "2152220001",
                              "newDist001@gmail.com",
                              "DISTRIBUTOR");

            order.Company.MemberStatus = StatusCode.ACTIVE.ToString();
            order.Company.MemberType = "DISTRIBUTOR";

            var newMemberType = string.Empty;
            var isNewMembership = order.IsNewMemberShip(ref newMemberType);
            Assert.IsTrue(isNewMembership);
        }

        #region Test company matching logic
        [Test]
        public void ReconcileCompanyMatchNameOnly()
        {
            var personify = new PersonifyService();
            List<string> masterIdList = null;

            //match name only  
            var company = GetStoreCompany("Reconcile Company Distributor 1",
                                          "1100000001",
                                          "noMatch@reconcile.com",
                                          "DISTRIBUTOR");
            GetPersonifyCompany(personify, company);

            company = GetStoreCompany("Reconcile Company Distributor 1",
                                      "1100000002",
                                      "noMatch1@reconcile.com",
                                      "DISTRIBUTOR");
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsNull(companyInfo);

            // match name with extra special characters and phone/email
            company = GetStoreCompany(".Reconcile $@#?Company $@#?$!&,Distributor 1.",
                                      "1100000001",
                                      "noMatch@reconcile.com",
                                      "DISTRIBUTOR");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Distributor 1");
        }

        [Test]
        public void MatchDistributorChange()
        {
            //Distributor should match to a delisted supplier
            //match name only  
            var personify = new PersonifyService();
            var company = GetStoreCompany("10 Again Inc",
                                          "818501800",
                                          "wholesale@10againclothing.com",
                                          "SUPPLIER");
            var companyInfo = GetPersonifyCompany(personify, company);
            Assert.IsNotNull(companyInfo);

            List<string> masterIdList = null;
            personify.UpdateCompanyStatus(company, StatusCode.DELISTED);
            company = GetStoreCompany("10 Again Inc",
                                      "818501801",
                                      "wholesale1@10againclothing.com",
                                      "SUPPLIER");

            AddPersonifyCompany(personify, company);
            var companyInfo1 = personify.FindCompanyInfo(company, ref masterIdList);

            Assert.IsTrue(companyInfo1 != null && companyInfo1.MasterCustomerId == companyInfo.MasterCustomerId);
            personify.UpdateCompanyStatus(company, StatusCode.LEAD);
        }

        [Test]
        public void ReconcileCompanySupplierWithPhoneEmail()
        {
            // match email only
            var company = GetStoreCompany("Reconcile Supplier",
                                          "1110011122",
                                          "supplierReconcile@reconcile.com",
                                          "SUPPLIER");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            var companyInfo = GetPersonifyCompany(personify, company);

            company = GetStoreCompany("Failed aaa Match",
                                      "1110011122",
                                      "supplierReconcile@reconcile.com",
                                      "SUPPLIER");
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Supplier");

            // match phone or email with LEAD matches
            company.Phone = "2135555552";
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");
        }

        [Test]
        public void ReconcileCompanyWithPhoneEmailForMMSLoad()
        {
            // match email only
            var company = GetStoreCompany("11x17 Inc",
                                          "9035410100",
                                          "jrw@11x17.com",
                                          "DISTRIBUTOR");

            IBackendService personify = new PersonifyService();
            var companyInfo = GetPersonifyCompany(personify, company);
            personify.UpdateCompanyStatus(company, StatusCode.MMS_LOAD);
            List<string> masterIdList = null;
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsNull(companyInfo);
            personify.UpdateCompanyStatus(company, StatusCode.LEAD);
        }

        [Test]
        public void ReconcileCompanyNonSupplierPhoneEmail()
        {
            var personify = new PersonifyService();
            //Distributor
            var company = GetStoreCompany("Reconcile Company Distributor 1",
                                          "2135555571",
                                          "individual3@reconcile.com",
                                          "DISTRIBUTOR");

            var companyInfo = GetPersonifyCompany(personify, company);

            List<string> masterIdList = null;
            company = GetStoreCompany("No matchhhh",
                                        "2135555571",
                                        "individual3@reconcile.com",
                                        "DISTRIBUTOR");
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Distributor 1");

            //Decorator 
            company = GetStoreCompany("Reconcile Company Decorator",
                                      "2135555511",
                                      "individual8@reconcile.com",
                                      "DECORATOR");

            companyInfo = GetPersonifyCompany(personify, company);
            company = GetStoreCompany("No matchhhh",
                                      "2135555511",
                                      "individual8@reconcile.com",
                                      "DECORATOR");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Decorator");
        }

        [Test]
        public void ReconcileCompanyMultipleMatches()
        {
            //Distributor matching both name and phone/email, one LEAD
            var company = GetStoreCompany("Reconcile Company Distributor 1",
                                          "2135555552",
                                          "individual3@reconcile.com",
                                          "DISTRIBUTOR");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            //Supplier match both name and phone/email, multiple LEAD with the latest note
            company = GetStoreCompany("Reconcile Company Supplier 1",
                                      "2135555551",
                                      "individual10@reconcile.com",
                                      "Supplier");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Supplier 1");
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            //Decorator matching name and email only
            company = GetStoreCompany("Reconcile Company Decorator",
                                      "2135555551",
                                      "individual8@reconcile.com",
                                      "DECORATOR");
            GetPersonifyCompany(personify, company);
            company.Name = "Reconcile Company Decorator 2";
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
        }

        [Test]
        public void ReconcileCompanySupplierNoMatch()
        {
            var company = GetStoreCompany("Invalid XXX Name",
                                          "11122111222",
                                          "uniqeEmail@reconcile.com",
                                          "SUPPLIER");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.AreEqual(companyInfo, null);
        }
        #endregion test company matching logic

        private StoreCompany GetStoreCompany(string name, string phone, string email, string memberType)
        {
            var company = new StoreCompany
            {
                Name = name,
                Phone = phone,
                MemberType = memberType
            };
            var address = new StoreAddress
            {
                Street1 = "4805 Street Rd",
                City = "Trevose",
                State = "PA",
                Country = "USA",
                Zip = "19053"
            };
            company.Addresses.Add(new StoreCompanyAddress
            {
                Address = address,
                IsBilling = true,
                IsShipping = true,
            });

            if (!string.IsNullOrEmpty(email))
            {
                company.Individuals = new List<StoreIndividual>()
                {
                    new StoreIndividual()
                      {   Email = email,
                          IsPrimary = true,
                          FirstName = "TestFirstName",
                          LastName = "TestLastName",
                          Address = address
                      }
                };
            }
            return company;
        }

        [Test]
        public void PersonifyAddActivity()
        {
            var activity = SvcClient.Create<CusActivity>();
            activity.MasterCustomerId = "000010252985";
            activity.SubCustomerId = 0;
            activity.ActivityCode = "CONTACTTRACKING";
            activity.CallTopicCode = "EXCEPTION";
            activity.CallTopicSubcode = "VALIDATION";
            activity.CallTypeCode = "STORE";
            activity.ActivityDate = DateTime.Now;
            activity.Subsystem = "MRM";
            activity.ActivityText = "Test personify activity Type and Subject.";

            var result = SvcClient.Save<CusActivity>(activity);
            Assert.AreEqual(result.CallTopicCode, "EXCEPTION");
            Assert.AreEqual(result.CallTopicSubcode, "VALIDATION");
            Assert.AreEqual(result.CallTypeCode, "STORE");
        }

        [Test]
        public void GetASICOMPMasterCustomerIdTest()
        {
            var acctid = "60019";
            PersonifyClient.GetASICOMPMasterCustomerId(acctid);

            acctid = "1002";
            PersonifyClient.GetASICOMPMasterCustomerId(acctid);

        }
        [Test]
        public void CheckValidEmailTest()
        {
            var email = "jshifflette@plowandhearth.com";
            var result = PersonifyClient.GetIndividualInfoByEmail(email, false);
            Assert.IsNotNull(result);
        }

        [Test]
        public void SaveCreditCardJetPayTest()
        {
            var parameters = new List<string>()
            {
                "000022867573",   // "@ip_master_customer_id",
                "0",    //"@ip_sub_customer_id",
                "ASI",    //"@ip_org_id",
                "ASI",    //"@ip_org_unit_id",
                "visa",    //"@ip_receipt_type_code",
                "May",   //"@ip_first_name",
                "test", //"@ip_last_name",
                "ASI TEsting",   //"@ip_company_name",
                    ////"@ip_cc_name",
                "4800 Street Rd",    //"@ip_address_1",
                "Trevose",    //"@ip_city",
                "PA",    //"@ip_state",
                "19053",    //"@ip_postal_code",
                "US",    //"@ip_country_code",
                "ASI_Store",    //"@ip_requester",
                "411111******1111",    //"@ip_cc_acct_no",
                "2023-10-01",   //"@ip_cc_exp_date",
                "USD",    //"@ip_currency_code",
                "ASIcompanies",   //"@ip_merchant_id",
                "126.45.789",    //"@ip_customer_ip_address",
                "QnTjTiPnRjPhQbQbRbSoRcSi",    //"@ip_cc_authorization",  // origianl paypal token
                "e7eec4f3ca50b89170",    //"@ip_auth_reference",
                "KNNJNIJNHJJHKBKBHBIOHCII",    //"@ip_request_token",      // new for JetPay
                "000",   //"@ip_response_code",      // new for JetPay 
                "TOKENIZED",    //"@ip_response_message",   // new for JetPay
                "U",    //"@ip_avs_result",         // new for JetPay                                              
                "WEBUSER",    //"@ip_user",
                "1",    //"@ip_prosessing_mode",  //0: Insert CC but it won’t show on Personify front-end
                    //                        //1: Insert CC and it will be listed on Personify front-end
                "0",    //"@ip_total_payment",
                null,    //    "@op_profile_id",
                "0",    //    "op_ErrNo",
                null,    //    "op_ErrMsg"
                    //}                
            };

            var result = PersonifyClient.ExecutePersonifySP("USR_OAM_INSERT_ASICCTP_PROC", parameters, false);
            Assert.IsNotNull(result);
        }

        private CompanyInformation GetPersonifyCompany(IBackendService personify, StoreCompany storeCompany)
        {
            personify = personify ?? new PersonifyService(MockupStoreService());
            var matchList = new List<string>();
            var companyInfo = personify.FindCompanyInfo(storeCompany, ref matchList);

            if (companyInfo == null)
            {
                companyInfo = AddPersonifyCompany(personify, storeCompany);
            }

            return companyInfo;
        }

        private CompanyInformation AddPersonifyCompany(IBackendService personify, StoreCompany storeCompany)
        {
            personify = personify ?? new PersonifyService(MockupStoreService());
            var contact = storeCompany.Individuals[0];
            var address = storeCompany.Addresses[0];
            var tag = DateTime.Now.Ticks;

            var accountInfo = new User
            {
                FirstName = contact.FirstName + tag.ToString().Substring(0, 4),
                LastName = contact.LastName + tag.ToString().Substring(0, 4),
                Email = contact.Email,
                CompanyName = storeCompany.Name,
                PhoneAreaCode = storeCompany.Phone.Substring(0, 3),
                Phone = storeCompany.Phone.Substring(3),
                Street1 = "4800 Street Rd",
                City = "Trevose",
                State = "PA",
                Zip = "19053",
                Country = "USA",
                CountryCode = "USA"
            };

            switch (storeCompany.MemberType)
            {
                case "DISTRIBUTOR":
                    accountInfo.MemberTypeId = 6;
                    break;
                case "DECORATOR":
                    accountInfo.MemberTypeId = 12;
                    break;
                case "SUPPLIER":
                    accountInfo.MemberTypeId = 7;
                    break;
            }

            var companyInfo = personify.AddCompany(accountInfo);
            Assert.IsTrue(companyInfo.CompanyId > 0);

            return companyInfo;
        }

        private IBackendService MockupPersonifyService(IStoreService storeService, StoreOrder storeOrder,
                                                       string memberStatus)
        {
            var mockPersonifyService = new Mock<PersonifyService>(storeService);
            mockPersonifyService.Setup(personifyService => personifyService.PlaceOrder(storeOrder, It.IsAny<IEmailService>(), string.Empty))
                                .Returns(new CompanyInformation() { MemberStatus = memberStatus });

            return mockPersonifyService.Object;
        }

        private IStoreService MockupStoreService(string couponCode = null)
        {
            var products = new List<ContextProduct>();
            products.Add(new ContextProduct { Id = 126, HasBackEndIntegration = true, Cost = 25 });
            products.Add(new ContextProduct { Id = 77, HasBackEndIntegration = true });
            products.Add(new ContextProduct { Id = 61, HasBackEndIntegration = true });
            products.Add(new ContextProduct { Id = 5, HasBackEndIntegration = true, Type = "Distributor Membership" });
            products.Add(new ContextProduct { Id = 2, HasBackEndIntegration = true, Type = "Supplier Membership", ApplicationCost = 250, Cost = 99 });
            products.Add(new ContextProduct { Id = 70, HasBackEndIntegration = true, Type = "Dual Distributor Decorator Membership" });
            products.Add(new ContextProduct { Id = 7, HasBackEndIntegration = true, Type = "Distributor Membership", ApplicationCost = 150, Cost = 199 });

            var emailExpresses = new List<StoreDetailEmailExpress>();
            emailExpresses.Add(new StoreDetailEmailExpress { OrderDetailId = 1, ItemTypeId = 1 });
            var mappings = new List<PersonifyMapping>();
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 126, StoreOption = "1", PersonifyProduct = 458162300, PersonifyRateCode = "500_credits", PersonifyRateStructure = "MEMBER", ProductCode = "Email_Marketing" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 77, StoreOption = "0", PersonifyProduct = 14471, PersonifyRateCode = "STD", PersonifyRateStructure = "MEMBER" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 61, StoreOption = "1;1X", PersonifyProduct = 1587, PersonifyRateCode = "1X", PersonifyRateStructure = "MEMBER" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 5, PersonifyProduct = 1003113722, PersonifyRateCode = "FY_DISTMEM", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 70, PersonifyProduct = 1003113722, PersonifyRateCode = "", PersonifyRateStructure = "MEMBER", ClassCode = "DECORATOER" });
            //mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 2, PersonifyProduct = 1003113955, StoreOption = "SUPWAVEAPPFEE", PersonifyRateCode = "FP_SUPMEMSTDCON", PersonifyRateStructure = "BUNDLE", PersonifyBundle = "SUPPLIERMEM_STD_CON" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 2, PersonifyProduct = 439255209, StoreOption = string.Empty, PersonifyRateCode = "FP_SUPMEMSTDCON", PersonifyRateStructure = "MEMBER", ClassCode = "SUPPLIER", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 2, PersonifyProduct = 439255209, StoreOption = string.Empty, PersonifyRateCode = "STD_SUPMEMSTD", PersonifyRateStructure = "MEMBER", ClassCode = "SUPPLIER", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 7, PersonifyProduct = 438951661, PersonifyRateCode = "FP_ESPPMDLMORD14", ProductCode = "ESPP-MD-LM-ORD", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = 8, StoreProduct = 7, StoreOption = couponCode, PersonifyProduct = 438951661, ProductCode = "ESPP-MD-LM-ORD", PersonifyRateCode = "SG_YR3_ESPPMDLMORD15", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = 8, StoreProduct = 7, StoreOption = couponCode, PersonifyRateCode = "STD", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR", PaySchedule = true });
            var codes = new List<LookSendMyAdCountryCode>();
            codes.Add(new LookSendMyAdCountryCode { Alpha2 = "USA", Alpha3 = "USA", CountryName = "United States" });

            var mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<ContextProduct>(true)).Returns(products.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<StoreDetailEmailExpress>(true)).Returns(emailExpresses.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<PersonifyMapping>(true)).Returns(mappings.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<LookSendMyAdCountryCode>(true)).Returns(codes.AsQueryable());

            if (!string.IsNullOrEmpty(couponCode))
            {
                var contexts = new List<Context>()
                {
                    new Context() { Id = 8, Products = new List<ContextProductSequence>(){ new ContextProductSequence () { Product = products.FirstOrDefault(p => p.Cost > 0), Cost = 199, ApplicationCost = 150 } } }
                };
                mockObjectService.Setup(objectService => objectService.GetAll<Context>(true)).Returns(contexts.AsQueryable());
            }

            return mockObjectService.Object;
        }

        private static StoreOrder CreateStoreOrder(string asiNumber, ContextProduct[] products, string testName,
                                              string couponCode = null, decimal discountAmount = 0, Context context = null)
        {
            var tag = DateTime.Now.Ticks;
            var address1 = new StoreAddress()
            {
                City = "Trevose",
                Country = "USA",
                State = "PA",
                Street1 = "Street1",
                Zip = "19053"
            };
            var address2 = new StoreAddress()
            {
                City = "Feasterville Trevose",
                Country = "USA",
                State = "PA",
                Street1 = "4800 Street Road",
                Zip = "19053",
            };
            var address3 = new StoreAddress()
            {
                City = "Feasterville Trevose",
                Country = "USA",
                State = "PA",
                Street1 = "Street2",
                Zip = "19053",
            };
            var companyAddresses = new List<StoreCompanyAddress>()
            {
                new StoreCompanyAddress() { Address = address1, IsBilling = false, IsShipping = false },
                new StoreCompanyAddress() { Address =  address2, IsShipping = true, IsBilling = false },
                new StoreCompanyAddress() { Address =  address3, IsShipping = false, IsBilling = true },
            };
            var person = new StoreIndividual()
            {
                Address = address1,
                IsPrimary = true,
                FirstName = string.IsNullOrEmpty(testName) ? "UnitTest" : testName,
                LastName = "UnitTest" + tag,
                Title = "Accountant",
                Email = "unitTest" + tag + "@gmail.com"
            };
            var contacts = new List<StoreIndividual>() { person };
            var company = new StoreCompany()
            {
                Name = string.Format("{0} {1}", string.IsNullOrEmpty(testName) ? "ORDER Test" : testName, tag),
                Addresses = companyAddresses,
                Individuals = contacts,
                ASINumber = asiNumber,
                Phone = "2153233242",
                WebURL = "http://asicentral.com/default.aspx"
            };
            var creditCard = new StoreCreditCard()
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                ExpMonth = "11",
                ExpYear = "2017",
                CardHolderName = "ASI Store",
            };

            var orderDetails = new List<StoreOrderDetail>();
            var order = new StoreOrder()
            {
                Company = company,
                AnnualizedTotal = 10,
                Total = 10,
                BillingIndividual = person,
                OrderDetails = orderDetails,
                OrderRequestType = "Supplier",
                CreditCard = creditCard
            };
            foreach (var product in products)
            {
                Coupon coupon = null;
                if (!string.IsNullOrEmpty(couponCode) && discountAmount > 0 && context != null)
                {
                    coupon = new Coupon() { ProductId = product.Id, CouponCode = couponCode, AppFeeDiscount = discountAmount };
                    order.ContextId = context.Id;
                }

                var orderDetail = new StoreOrderDetail()
                {
                    Id = 1,
                    ApplicationCost = product.ApplicationCost,
                    Cost = product.Cost,
                    OptionId = 0,
                    Quantity = 1,
                    IsSubscription = true,
                    Product = product,
                    Order = order,
                    DateOption = DateTime.Now.AddDays(2),
                    Coupon = coupon
                };
                orderDetails.Add(orderDetail);
            }
            return order;
        }

        private CompanyInformation CreatePersonifyOrder(StoreOrder order, IStoreService storeService)
        {
            IBackendService personify = new PersonifyService(storeService);
            return CreatePersonifyOrder(order, storeService, personify);
        }

        private CompanyInformation CreatePersonifyOrder(StoreOrder order, IStoreService storeService, IBackendService personify)
        {
            //simulate the store process by first processing the credit card
            var personifyService = new PersonifyService(storeService);
            var cc = new CreditCard
            {
                Address = "",
                CardHolderName = order.CreditCard.CardHolderName,
                Type = order.CreditCard.CardType,
                Number = order.CreditCard.CardNumber,
                ExpirationDate = new DateTime(int.Parse(order.CreditCard.ExpYear), int.Parse(order.CreditCard.ExpMonth), 1),
            };
            var profileIdentifier = personifyService.SaveCreditCard(order, cc);
            Assert.IsNotNull(profileIdentifier);
            Assert.IsNotNull(order.Company.ExternalReference);
            order.CreditCard.ExternalReference = profileIdentifier;
            return personify.PlaceOrder(order, new Mock<IEmailService>().Object, string.Empty);
        }
    }
}
