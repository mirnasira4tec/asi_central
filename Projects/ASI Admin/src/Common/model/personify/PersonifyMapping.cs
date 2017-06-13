using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.personify
{
    public enum PersonifyStatus
    {
        Success = 0,
        NoRecordFound = 1,
        PersonifyError = 2
    }

    public class PersonifyMapping
    {
        public PersonifyMapping()
        {
            ItemCount = 1;
            Quantity = 1;
        }

        public Guid Identifier { get; set; }
        public int? StoreContext { get; set; }
        public int? StoreProduct { get; set; }
        public string StoreOption { get; set; }
        public string ClassCode { get; set; }
        public string SubClassCode { get; set; }
        //public string StoreCouponId { get; set; }
        public int? PersonifyProduct { get; set; }
        public string ProductCode { get; set; }
        public bool? PaySchedule { get; set; }
        public string PersonifyBundle { get; set; }
        public string PersonifyRateCode { get; set; }
        public string PersonifyRateStructure { get; set; }
        public bool ESBSendGlag { get; set; }
        public bool NewAsiNumFlag { get; set; }
        public bool NotifyByEmailFlag { get; set; }
        public string CreateUserUTC { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime? UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        //not in the database but used for the process
        [NotMapped]
        public int ItemCount { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
    }
}
