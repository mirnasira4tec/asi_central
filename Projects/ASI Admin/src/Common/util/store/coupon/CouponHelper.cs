using asi.asicentral.interfaces;
using asi.asicentral.model.personify;
using asi.asicentral.model.store;
using System.Collections.Generic;
using System.Linq;


namespace asi.asicentral.util.store.coupon
{
    public static class CouponHelper
    {

        public static readonly Dictionary<int, List<string>> WAIVE_APP_FEE_RATECODES =
                                                    new Dictionary<int, List<string>>()
                                                    {
                                                        {160, new List<string>(){"STD", "SPECIAL"}}, 
                                                        {159, new List<string>(){"STD", "SPECIAL"}},
                                                        {4896, new List<string>(){"STD", "SPECIAL"}}
                                                    };

        public static readonly Dictionary<int, List<string>> FREE_MONTH_RATECODES =
                                            new Dictionary<int, List<string>>()
                                                    {
                                                        {1003113981,new List<string>(){"STD", "TRIAL_13"}}
                                                    };

        public static bool IsValidCoupon(IStoreService storeService, Coupon coupon)
        {
            var mappings = storeService.GetAll<PersonifyMapping>()
                                 .Where(map => map.StoreOption == coupon.CouponCode).ToList();

            return mappings.Any();
        }
    }
}
