using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class DistributorApplicationModel : StoreDetailDistributorMembership, IMembershipModel
    {
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "CompanyName")]
        public string Company { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street1")]
        public string Address1 { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street2")]
        public string Address2 { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "City")]
        public string City { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Zipcode")]
        public string Zip { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "State")]
        public string State { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Country")]
        public string Country { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Phone")]
        public string Phone { get; set; }
        public string InternationalPhone { get; set; }
        [RegularExpression(@"^[1-9][0-9]{3,5}$", ErrorMessageResourceName = "FieldInvalidASINumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        [StringLength(6, ErrorMessageResourceType = typeof(asi.asicentral.Resource), ErrorMessageResourceName = "FieldLength")]
        public string ASINumber { get; set; }
        public bool HasShipAddress { get; set; }
        public bool HasBillAddress { get; set; }

        #region Billing information

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "BillingTollPhone")]
        public string BillingTollFree { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Fax")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        public string BillingFax { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street1")]
        public string BillingAddress1 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street2")]
        public string BillingAddress2 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "City")]
        public string BillingCity { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "State")]
        public string BillingState { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Zipcode")]
        public string BillingZip { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Country")]
        public string BillingCountry { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Phone")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        public string BillingPhone { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string BillingEmail { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "WebUrl")]
        [DataType(DataType.Url)]
        public string BillingWebUrl { get; set; }

        #endregion Billing information

        #region shipping information

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingAddress")]
        public string ShippingStreet1 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingAddress2")]
        public string ShippingStreet2 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingCity")]
        public string ShippingCity { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingState")]
        public string ShippingState { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingZip")]
        public string ShippingZip { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Country")]
        public string ShippingCountry { get; set; }

        #endregion shipping information

        public IList<StoreIndividual> Contacts { get; set; }
        public string BuisnessRevenue { set; get; }

        public bool Signs { set; get; }
        public bool TrophyAwards { set; get; }
        public bool Printing { set; get; }
        public bool ScreenPrinting { set; get; }
        public bool PromotionalProducts { set; get; }
        public bool Other { set; get; }
        public decimal MonthlyPrice { get; set; }
        public decimal Price { get; set; }
        
        public bool ProductA { get; set; }
        public bool ProductB { get; set; }
        public bool ProductC { get; set; }
        public bool Product1 { get; set; }
        public bool ProductD { get; set; }
        public bool ProductO { get; set; }
        public bool ProductY { get; set; }
        public bool ProductZ { get; set; }
        public bool ProductL { get; set; }
        public bool ProductG { get; set; }
        public bool ProductF { get; set; }
        public bool ProductI { get; set; }
        public bool ProductV { get; set; }
        public bool ProductJ { get; set; }
        public bool ProductH { get; set; }
        public bool ProductK { get; set; }
        public bool ProductU { get; set; }
        public bool ProductX { get; set; }
        public bool ProductM { get; set; }
        public bool ProductN { get; set; }
        public bool Product3 { get; set; }
        public bool Product4 { get; set; }
        public bool Product5 { get; set; }
        public bool ProductE { get; set; }
        public bool ProductP { get; set; }
        public bool ProductQ { get; set; }
        public bool ProductW { get; set; }
        public bool Product2 { get; set; }
        public bool ProductR { get; set; }
        public bool ProductS { get; set; }
        public bool ProductT { get; set; }

        public bool AccountA { get; set; }
        public bool AccountV { get; set; }
        public bool AccountK { get; set; }
        public bool AccountS { get; set; }
        public bool AccountB { get; set; }
        public bool AccountI { get; set; }
        public bool AccountJ { get; set; }
        public bool AccountC { get; set; }
        public bool AccountD { get; set; }
        public bool AccountT { get; set; }
        public bool AccountW { get; set; }
        public bool AccountF { get; set; }
        public bool AccountH { get; set; }
        public bool AccountO { get; set; }
        public bool AccountX { get; set; }
        public bool AccountY { get; set; }
        public bool AccountL { get; set; }
        public bool AccountM { get; set; }
        public bool AccountU { get; set; }
        public bool AccountN { get; set; }
        public bool AccountP { get; set; }
        public bool AccountE { get; set; }
        public bool AccountZ { get; set; }
        public bool AccountQ { get; set; }
        public bool Account1 { get; set; }
        public bool AccountG { get; set; }
        public bool AccountR { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        public DistributorApplicationModel() : base()
        {
            this.ProductLines = new List<LookProductLine>();
            this.AccountTypes = new List<LookDistributorAccountType>();
        }

        public DistributorApplicationModel(StoreDetailDistributorMembership application, StoreOrderDetail orderDetail)
        {
            StoreOrder order = orderDetail.Order;
            application.CopyTo(this);
            GetPrimaryBusinessRevenue();
            GetProductLines();
            GetAccountTypes();
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Completed = order.IsCompleted;
            Price = order.Total;
            MonthlyPrice = (order.Total - order.AnnualizedTotal) / 11;
            MembershipModelHelper.PopulateModel(this, order);
        }

        private void GetPrimaryBusinessRevenue()
        {
            Signs = HasPrimaryBuisnessRevenue(LegacyDistributorBusinessRevenue.BUSINESSREVENUE_SIGNS);
            TrophyAwards = HasPrimaryBuisnessRevenue(LegacyDistributorBusinessRevenue.BUSINESSREVENUE_TROPHYAWARDS);
            Printing = HasPrimaryBuisnessRevenue(LegacyDistributorBusinessRevenue.BUSINESSREVENUE_PRINTING);
            ScreenPrinting = HasPrimaryBuisnessRevenue(LegacyDistributorBusinessRevenue.BUSINESSREVENUE_SCREENPRINTING);
            PromotionalProducts = HasPrimaryBuisnessRevenue(LegacyDistributorBusinessRevenue.BUSINESSREVENUE_PROMOTIONALPRODUCTS);
            Other = string.IsNullOrEmpty(this.OtherBusinessRevenue) ? false : true;
        }

        private void AddType(bool selected, String codeName, IList<LookDistributorAccountType> accountTypes, StoreDetailDistributorMembership application)
        {
            LookDistributorAccountType reference = application.AccountTypes.Where(type => type.SubCode == codeName).SingleOrDefault();
            if (selected && reference == null) application.AccountTypes.Add(accountTypes.Where(type => type.SubCode == codeName).SingleOrDefault());
            else if (!selected && reference != null) application.AccountTypes.Remove(reference);
        }

        private void AddProductLine(bool selected, String codeName, IList<LookProductLine> productLines, StoreDetailDistributorMembership application)
        {
            LookProductLine reference = application.ProductLines.Where(type => type.SubCode == codeName && type.MemberType == "distributor").SingleOrDefault();
            if (selected && reference == null) application.ProductLines.Add(productLines.Where(type => type.SubCode == codeName && type.MemberType == "distributor").SingleOrDefault());
            else if (!selected && reference != null) application.ProductLines.Remove(reference);
        }

        public void SyncProductLinesToApplication(IList<LookProductLine> productLines, StoreDetailDistributorMembership application)
        {
            AddProductLine(Product1, "1", productLines, application);
            AddProductLine(Product2, "2", productLines, application);
            AddProductLine(Product3, "3", productLines, application);
            AddProductLine(Product4, "4", productLines, application);
            AddProductLine(Product5, "5", productLines, application);
            AddProductLine(ProductA, "A", productLines, application);
            AddProductLine(ProductB, "B", productLines, application);
            AddProductLine(ProductC, "C", productLines, application);
            AddProductLine(ProductD, "D", productLines, application);
            AddProductLine(ProductE, "E", productLines, application);
            AddProductLine(ProductF, "F", productLines, application);
            AddProductLine(ProductG, "G", productLines, application);
            AddProductLine(ProductH, "H", productLines, application);
            AddProductLine(ProductI, "I", productLines, application);
            AddProductLine(ProductJ, "J", productLines, application);
            AddProductLine(ProductK, "K", productLines, application);
            AddProductLine(ProductL, "L", productLines, application);
            AddProductLine(ProductM, "M", productLines, application);
            AddProductLine(ProductN, "N", productLines, application);
            AddProductLine(ProductO, "O", productLines, application);
            AddProductLine(ProductP, "P", productLines, application);
            AddProductLine(ProductQ, "Q", productLines, application);
            AddProductLine(ProductR, "R", productLines, application);
            AddProductLine(ProductS, "S", productLines, application);
            AddProductLine(ProductT, "T", productLines, application);
            AddProductLine(ProductU, "U", productLines, application);
            AddProductLine(ProductV, "V", productLines, application);
            AddProductLine(ProductW, "W", productLines, application);
            AddProductLine(ProductX, "X", productLines, application);
            AddProductLine(ProductY, "Y", productLines, application);
            AddProductLine(ProductZ, "Z", productLines, application);
        }

        public void SyncAccountTypesToApplication(IList<LookDistributorAccountType> accountTypes, StoreDetailDistributorMembership application)
        {
            AddType(Account1, "1", accountTypes, application);
            AddType(AccountA, "A", accountTypes, application);
            AddType(AccountB, "B", accountTypes, application);
            AddType(AccountC, "C", accountTypes, application);
            AddType(AccountD, "D", accountTypes, application);
            AddType(AccountE, "E", accountTypes, application);
            AddType(AccountF, "F", accountTypes, application);
            AddType(AccountG, "G", accountTypes, application);
            AddType(AccountH, "H", accountTypes, application);
            AddType(AccountI, "I", accountTypes, application);
            AddType(AccountJ, "J", accountTypes, application);
            AddType(AccountK, "K", accountTypes, application);
            AddType(AccountL, "L", accountTypes, application);
            AddType(AccountM, "M", accountTypes, application);
            AddType(AccountN, "N", accountTypes, application);
            AddType(AccountO, "O", accountTypes, application);
            AddType(AccountP, "P", accountTypes, application);
            AddType(AccountQ, "Q", accountTypes, application);
            AddType(AccountR, "R", accountTypes, application);
            AddType(AccountS, "S", accountTypes, application);
            AddType(AccountT, "T", accountTypes, application);
            AddType(AccountU, "U", accountTypes, application);
            AddType(AccountV, "V", accountTypes, application);
            AddType(AccountW, "W", accountTypes, application);
            AddType(AccountX, "X", accountTypes, application);
            AddType(AccountY, "Y", accountTypes, application);
            AddType(AccountZ, "Z", accountTypes, application);
        }

        private void GetAccountTypes()
        {
            Account1 = HasAccountType("1");
            AccountA = HasAccountType("A");
            AccountB = HasAccountType("B");
            AccountC = HasAccountType("C");
            AccountD = HasAccountType("D");
            AccountE = HasAccountType("E");
            AccountF = HasAccountType("F");
            AccountG = HasAccountType("G");
            AccountH = HasAccountType("H");
            AccountI = HasAccountType("I");
            AccountJ = HasAccountType("J");
            AccountK = HasAccountType("K");
            AccountL = HasAccountType("L");
            AccountM = HasAccountType("M");
            AccountN = HasAccountType("N");
            AccountO = HasAccountType("O");
            AccountP = HasAccountType("P");
            AccountQ = HasAccountType("Q");
            AccountR = HasAccountType("R");
            AccountS = HasAccountType("S");
            AccountT = HasAccountType("T");
            AccountU = HasAccountType("U");
            AccountV = HasAccountType("V");
            AccountW = HasAccountType("W");
            AccountX = HasAccountType("X");
            AccountY = HasAccountType("Y");
            AccountZ = HasAccountType("Z");
        }

        private void GetProductLines()
        {
            Product1 = HasProductLine("1");
            Product2 = HasProductLine("2");
            Product3 = HasProductLine("3");
            Product4 = HasProductLine("4");
            Product5 = HasProductLine("5");
            ProductA = HasProductLine("A");
            ProductB = HasProductLine("B");
            ProductC = HasProductLine("C");
            ProductD = HasProductLine("D");
            ProductE = HasProductLine("E");
            ProductF = HasProductLine("F");
            ProductG = HasProductLine("G");
            ProductH = HasProductLine("H");
            ProductI = HasProductLine("I");
            ProductJ = HasProductLine("J");
            ProductK = HasProductLine("K");
            ProductL = HasProductLine("L");
            ProductM = HasProductLine("M");
            ProductN = HasProductLine("N");
            ProductO = HasProductLine("O");
            ProductP = HasProductLine("P");
            ProductQ = HasProductLine("Q");
            ProductR = HasProductLine("R");
            ProductS = HasProductLine("S");
            ProductT = HasProductLine("T");
            ProductU = HasProductLine("U");
            ProductV = HasProductLine("V");
            ProductW = HasProductLine("W");
            ProductX = HasProductLine("X");
            ProductY = HasProductLine("Y");
            ProductZ = HasProductLine("Z");
        }

        private bool HasAccountType(string code)
        {
            return (this.AccountTypes.Where(accountType => accountType.SubCode == code).Count() == 1);
        }

        private bool HasProductLine(string code)
        {
            return (this.ProductLines.Where(productLines => productLines.SubCode == code).Count() == 1);
        }

        private bool HasPrimaryBuisnessRevenue(string name)
        {
            if (this.PrimaryBusinessRevenue != null) return (this.PrimaryBusinessRevenue.Name == name);
            else return false;
        }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool Completed { get; set; }
    }
}