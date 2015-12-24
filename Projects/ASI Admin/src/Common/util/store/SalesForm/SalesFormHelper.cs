namespace asi.asicentral.util.store
{
	public static class SalesFormHelper
	{
		public static readonly string[,] PRODUCTS = { 
            { "ESP Website", "ESPW-60", "ESP (ALL) Product" }, 
            { "Additional ESP License", "ESPL-60", "" }, 
            { "Company Store", "CPYS-49.99", "ESP (ALL) Product" },
            { "Distributor Membership", "DIST-49.99", "Distributor Membership" }, 
            { "Distributor Package – Basic","DIST-149.99", "Distributor Membership" }, 
            { "Distributor Package - Standard", "DIST-199.99", "Distributor Membership;ESP (ALL) Product" },
            { "Distributor Package – Executive", "DIST-229.99", "Distributor Membership;ESP (ALL) Product" }, 
            { "Distributor Package - Premium", "DIST-299.99", "Distributor Membership;ESP (ALL) Product" },
			{ "ESP Social – Starter", "ESPS-49.99", "ESP (ALL) Product" }, 
            { "ESP Social – Advanced","ESPS-99.99", "ESP (ALL) Product" },
            { "ESP Social – Pro", "ESPS-199.99", "ESP (ALL) Product" }, 
            { "ASI Brand Builder - Standard", "ABRB-399.99", "ESP (ALL) Product" }, 
            { "ASI Brand Builder - Plus", "ABRB-599.99", "ESP (ALL) Product" }, 
            { "ASI Brand Builder - Platinum", "ABRB-999.99", "ESP (ALL) Product" },
			{ "Trafficbuilder Plus – Standard", "TFBP-499.99", "ESP (ALL) Product" }, 
            { "Trafficbuilder Plus – Pro", "TFBP-999.99", "ESP (ALL) Product" }, 
            { "Trafficbuilder Plus – Platinum", "TFBP-1000.00", "ESP (ALL) Product" }, 
            { "Trafficbuilder Plus- Local SEO Only", "TFBP-99.00", "ESP (ALL) Product" },
			{ "Decorator Membership - Standard", "DECM-49.99", "Decorator Membership" }, 
            { "Decorator Membership - Plus", "DECM-99.99", "Decorator Membership" }, 
            { "Decorator Membership - Pro", "DECM-149.99", "Decorator Membership" } };

		public static readonly string[,] FEES = { { "Application Fee", "APPF-150" }, { "Set Up Fee", "STPF-199" }, 
             { "Custom Package Pro", "CPRO-299" }, { "Custom Package Master", "CPMT-599" },  { "Custom Package Genius", "CPGE-999" } };

        public static readonly int SALES_FORM_PRODUCT_ID = 99;

		public static string GetProductDescription(string code)
		{
			var description = string.Empty;
			for (var i = 0; i < PRODUCTS.GetLength(0); i++)
			{
				if (PRODUCTS[i,1].Equals(code))
				{
					description = PRODUCTS[i, 0];
					break;
				}
			}
			if (description.Equals(string.Empty))
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
			return description;
		}
	}
}
