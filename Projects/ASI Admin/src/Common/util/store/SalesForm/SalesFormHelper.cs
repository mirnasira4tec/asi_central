namespace asi.asicentral.util.store
{
    public static class SalesFormHelper
    {
        public static readonly string[,] PRODUCTS = {
            { "ESP Website", "ESPW-60", "ESP (ALL) Product" },
            { "Additional ESP License", "ESPL-60", "" },
            { "Company Store", "CPYS-49.99", "ESP (ALL) Product" },
            { "Distributor Membership", "DIST-29.99", "Distributor Membership" },
            { "Distributor Package - Corporate Account","DTCA-199.99", "Distributor Membership;ESP (ALL) Product" },
            { "Distributor Package – Basic","DIST-149.99", "Distributor Membership" },
            { "Distributor Package - Standard", "DIST-199.99", "Distributor Membership;ESP (ALL) Product" },
            { "Distributor Package – Executive", "DIST-219.99", "Distributor Membership;ESP (ALL) Product" },
            { "Distributor Package - Premium", "DIST-299.99", "Distributor Membership;ESP (ALL) Product" },
            { "Pay Per Click Advertising plus monthly advertising spend", "PPCA-149",""},
            { "Company Reviews","CORW-99",""},
            { "Local SEO" ,"LOSE-99",""},
            { "Local SEO and Reviews" ,"LOSE-149",""},
            { "Local SEO, PPC and Reviews plus monthly advertising spend" ,"LOSE-229",""},
            { "Social Boost – Starter", "SOBO-49",""},
            { "Social Boost – Advanced", "SOBO-99",""},
            { "Social Boost – Pro", "SOBO-199",""},
            { "Corporate Gifts and Incentives","CG&I-20",""},
            { "Basic ESPWS Maintenance 1","BMWP-20",""},
            { "Basic ESPWS Maintenance 2","BMWP-40",""},
            { "Basic ESPWS Maintenance 3","BMWP-60",""},
            { "Custom ESPWS Maintenance 1","CMWP-20",""},
            { "Custom ESPWS Maintenance 2","CMWP-40",""},
            { "Custom ESPWS Maintenance 3","CMWP-60",""},
            { "Decorator Membership - Standard", "DECM-49.99", "Decorator Membership" },
            { "Decorator Membership - Plus", "DECM-99.99", "Decorator Membership" },
            { "Decorator Membership - Pro", "DECM-149.99", "Decorator Membership" } };

        public static readonly string[,] ACTIVE_PRODUCTS = {
            { "Additional ESP License", "ESPL-60", "" },
            { "Distributor Membership", "DIST-29.99", "Distributor Membership" },
            { "Distributor Package – Basic","DIST-149.99", "Distributor Membership" },
            { "Distributor Package - Standard", "DIST-199.99", "Distributor Membership;ESP (ALL) Product" },
            { "Distributor Package – Executive", "DIST-219.99", "Distributor Membership;ESP (ALL) Product" },
            { "Distributor Package - Premium", "DIST-299.99", "Distributor Membership;ESP (ALL) Product" },
            { "Decorator Membership - Standard", "DECM-49.99", "Decorator Membership" },
            { "Decorator Membership - Plus", "DECM-99.99", "Decorator Membership" },
            { "Decorator Membership - Pro", "DECM-149.99", "Decorator Membership" } };

        public static readonly string[,] FEES = {
            { "Application Fee", "APPF-150", "" },
            { "Set Up Fee", "STPF-199", "" },
            { "Custom Package Pro", "CPRO-599", "" },
            { "Custom Package Genius", "CPGE-999", "" },
            { "Custom Package Prodigy", "CPRD-1499", "" },
            { "Custom Package Virtuoso", "CPGE-1899", "" },
            { "Custom Package", "CPOT-0", "" } };

        public static readonly string[,] OLD_PRODUCTS = {
            { "Website Maintenance 1 hr","WEBM-120",""},
            { "Website Maintenance 2 hr","WEBM-240",""},
            { "Website Maintenance 3 hr","WEBM-360",""},
            { "Website Maintenance 4 hr","WEBM-480",""},
            { "Website Maintenance 6 hr","WEBM-720",""},
            { "Website Maintenance 12 hr","WEBM-1440",""}};

        public static readonly int SALES_FORM_PRODUCT_ID = 99;

        public static string GetProductDescription(string code)
        {
            var description = string.Empty;
            for (var i = 0; i < PRODUCTS.GetLength(0); i++)
            {
                if (PRODUCTS[i, 1].Equals(code))
                {
                    description = PRODUCTS[i, 0];
                    break;
                }
            }
            if (string.IsNullOrEmpty(description))
            {
                for (var i = 0; i < FEES.GetLength(0); i++)
                {
                    if (FEES[i, 1].Equals(code))
                    {
                        description = FEES[i, 0];
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(description))
            {
                for (var i = 0; i < OLD_PRODUCTS.GetLength(0); i++)
                {
                    if (OLD_PRODUCTS[i, 1].Equals(code))
                    {
                        description = OLD_PRODUCTS[i, 0];
                        break;
                    }
                }
            }
            return description;
        }
    }
}
