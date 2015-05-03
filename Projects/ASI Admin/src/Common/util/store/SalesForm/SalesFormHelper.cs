namespace asi.asicentral.util.store
{
	public static class SalesFormHelper
	{
		public static readonly string[,] PRODUCTS = { { "ESP Website", "ESPW-60" }, { "Additional ESP License", "ESPL-60" }, { "Company Store", "CPYS-49.99" }, 
			{ "Distributor Membership", "DIST-49.99" }, {"Distributor Package – Basic","DIST-149.99"}, {"Distributor Package - Standard","DIST-199.99"}, {"Distributor Package – Executive","DIST-229.99"}, {"Distributor Package - Premium","DIST-299.99"},
			{ "ESP Social – Starter", "ESPS-49.99"}, {"ESP Social – Advanced","ESPS-99.99"}, {"ESP Social – Pro", "ESPS-199.99"},
			{"ASI Brand Builder - Standard", "ABRB-399.99"}, {"ASI Brand Builder - Plus", "ABRB-599.99"}, {"ASI Brand Builder - Platinum", "ABRB-999.99"},
			{"Trafficbuilder Plus – Standard", "TFBP-499.99"}, {"Trafficbuilder Plus – Pro", "TFBP-999.99"}, {"Trafficbuilder Plus – Platinum", "TFBP-1000.00"}, {"Trafficbuilder Plus- Local SEO Only", "TFBP-100.00"},
			{"Decorator Membership - Standard","DECM-49.99"}, {"Decorator Membership - Plus", "DECM-99.99"}, {"Decorator Membership - Pro", "DECM-149.99"}};

		public static readonly string[,] FEES = { { "Application Fee", "APPF-150" }, { "Set Up Fee", "STPF-199" } };
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
