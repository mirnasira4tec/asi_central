using asi.asicentral.interfaces;
using asi.asicentral.model.ROI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class ROIService : IROIService
    {
        public Category[] GetImpressionsPerCategory(int asiNumber)
        {
            return GetImpressionsPerCategoryTemplate();
        }

        private Category[] GetImpressionsPerCategoryTemplate()
        {
            Category[] categories = new Category[3];
            categories[0] = new Category() { Name = "BABY ITEMS - DEMO", SupplierCount = 217 };
            categories[0].Results = new Company[7];
            categories[0].Results[0] = new Company() { IsRequestor = false, Impressions = 1143, Rank = 1 };
            categories[0].Results[1] = new Company() { IsRequestor = false, Impressions = 1143, Rank = 1 };
            categories[0].Results[2] = new Company() { IsRequestor = false, Impressions = 1026, Rank = 3 };
            categories[0].Results[3] = new Company() { IsRequestor = false, Impressions = 846, Rank = 4 };
            categories[0].Results[4] = new Company() { IsRequestor = false, Impressions = 829, Rank = 5 };
            categories[0].Results[5] = new Company() { IsRequestor = false, Impressions = 700, Rank = 6 };
            categories[0].Results[6] = new Company() { IsRequestor = true, Impressions = 35, Rank = 81 };
            categories[0].Requestor = categories[0].Results[6];

            categories[1] = new Category() { Name = "BASEBALL CAPS - DEMO", SupplierCount = 357 };
            categories[1].Results = new Company[7];
            categories[1].Results[0] = new Company() { IsRequestor = false, Impressions = 8207, Rank = 1 };
            categories[1].Results[1] = new Company() { IsRequestor = false, Impressions = 7497, Rank = 2 };
            categories[1].Results[2] = new Company() { IsRequestor = false, Impressions = 6015, Rank = 3 };
            categories[1].Results[3] = new Company() { IsRequestor = false, Impressions = 5580, Rank = 4 };
            categories[1].Results[4] = new Company() { IsRequestor = false, Impressions = 5429, Rank = 5 };
            categories[0].Results[5] = new Company() { IsRequestor = false, Impressions = 4326, Rank = 6 };
            categories[1].Results[6] = new Company() { IsRequestor = true, Impressions = 272, Rank = 66 };
            categories[1].Requestor = categories[1].Results[6];

            categories[2] = new Category() { Name = "GOLF/POLO SHIRTS - DEMO", SupplierCount = 262 };
            categories[2].Results = new Company[7];
            categories[2].Results[0] = new Company() { IsRequestor = false, Impressions = 33218, Rank = 1 };
            categories[2].Results[1] = new Company() { IsRequestor = false, Impressions = 8981, Rank = 2 };
            categories[2].Results[2] = new Company() { IsRequestor = false, Impressions = 5127, Rank = 3 };
            categories[2].Results[3] = new Company() { IsRequestor = false, Impressions = 4109, Rank = 4 };
            categories[2].Results[4] = new Company() { IsRequestor = false, Impressions = 4055, Rank = 5 };
            categories[0].Results[5] = new Company() { IsRequestor = false, Impressions = 3943, Rank = 6 };
            categories[2].Results[6] = new Company() { IsRequestor = true, Impressions = 919, Rank = 31 };
            categories[2].Requestor = categories[2].Results[6];
            return categories;
        }
    }
}
