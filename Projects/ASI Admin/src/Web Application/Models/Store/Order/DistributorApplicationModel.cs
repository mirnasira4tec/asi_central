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
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
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

        private void AddType(bool selected, String codeName, IList<LookDistributorAccountType> accountTypes)
        {
            if (selected)
            {
                LookDistributorAccountType account = accountTypes.Where(type => type.SubCode == codeName).SingleOrDefault();
                if (account != null) this.AccountTypes.Add(account);
            }
        }

        private void AddProductLine(bool selected, String codeName, IList<LookProductLine> productLines)
        {
            if (selected)
            {
                LookProductLine line = productLines.Where(productline => productline.SubCode == codeName).SingleOrDefault();
                if (line != null) this.ProductLines.Add(line);
            }
        }

        public void SyncProductLinesFrom(IList<LookProductLine> productLines)
        {
            AddProductLine(Product1, "1", productLines);
            AddProductLine(Product2, "2", productLines);
            AddProductLine(Product3, "3", productLines);
            AddProductLine(Product4, "4", productLines);
            AddProductLine(Product5, "5", productLines);
            AddProductLine(ProductA, "A", productLines);
            AddProductLine(ProductB, "B", productLines);
            AddProductLine(ProductC, "C", productLines);
            AddProductLine(ProductD, "D", productLines);
            AddProductLine(ProductE, "E", productLines);
            AddProductLine(ProductF, "F", productLines);
            AddProductLine(ProductG, "G", productLines);
            AddProductLine(ProductH, "H", productLines);
            AddProductLine(ProductI, "I", productLines);
            AddProductLine(ProductJ, "J", productLines);
            AddProductLine(ProductK, "K", productLines);
            AddProductLine(ProductL, "L", productLines);
            AddProductLine(ProductM, "M", productLines);
            AddProductLine(ProductN, "N", productLines);
            AddProductLine(ProductO, "O", productLines);
            AddProductLine(ProductP, "P", productLines);
            AddProductLine(ProductQ, "Q", productLines);
            AddProductLine(ProductR, "R", productLines);
            AddProductLine(ProductS, "S", productLines);
            AddProductLine(ProductT, "T", productLines);
            AddProductLine(ProductU, "U", productLines);
            AddProductLine(ProductV, "V", productLines);
            AddProductLine(ProductW, "W", productLines);
            AddProductLine(ProductX, "X", productLines);
            AddProductLine(ProductY, "Y", productLines);
            AddProductLine(ProductZ, "Z", productLines);

        }

        public void SyncAccountTypesFrom(IList<LookDistributorAccountType> accountTypes)
        {
            AddType(Account1, "1", accountTypes);
            AddType(AccountA, "A", accountTypes);
            AddType(AccountB, "B", accountTypes);
            AddType(AccountC, "C", accountTypes);
            AddType(AccountD, "D", accountTypes);
            AddType(AccountE, "E", accountTypes);
            AddType(AccountF, "F", accountTypes);
            AddType(AccountG, "G", accountTypes);
            AddType(AccountH, "H", accountTypes);
            AddType(AccountI, "I", accountTypes);
            AddType(AccountJ, "J", accountTypes);
            AddType(AccountK, "K", accountTypes);
            AddType(AccountL, "L", accountTypes);
            AddType(AccountM, "M", accountTypes);
            AddType(AccountN, "N", accountTypes);
            AddType(AccountO, "O", accountTypes);
            AddType(AccountP, "P", accountTypes);
            AddType(AccountQ, "Q", accountTypes);
            AddType(AccountR, "R", accountTypes);
            AddType(AccountS, "S", accountTypes);
            AddType(AccountT, "T", accountTypes);
            AddType(AccountU, "U", accountTypes);
            AddType(AccountV, "V", accountTypes);
            AddType(AccountW, "W", accountTypes);
            AddType(AccountX, "X", accountTypes);
            AddType(AccountY, "Y", accountTypes);
            AddType(AccountZ, "Z", accountTypes);
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