using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public abstract class StoreDetailApplication
    {
        public int OrderDetailId { get; set; }
        public string LegacyApplicationId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ApplicationStatus")]
        public Nullable<int> AppStatusId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public void CopyTo(StoreDetailApplication application) 
        {
            application.OrderDetailId = OrderDetailId;
            application.LegacyApplicationId = LegacyApplicationId;
            application.AppStatusId = AppStatusId;
            application.CreateDate = CreateDate;
            application.UpdateDate = UpdateDate;
            application.UpdateSource = UpdateSource;
        }
    }
}
