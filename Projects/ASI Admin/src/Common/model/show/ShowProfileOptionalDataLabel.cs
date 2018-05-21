using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
   public class ShowProfileOptionalDataLabel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public bool? IsObsolete { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsDistributor { get; set; }
    }
    public enum ProfileData
    {
        Unknown = 0,
        AttendeeName = 1,
        AttendeeTitle = 2,
        AttendeeCommEmail = 3,
        AttendeeCellPhone = 4,
        AttendeeWorkPhone = 5,
        AttendeeImage = 6,
        OfferToDistributor = 7,
        GoalForParticipating = 8,
        FiveDistributorCompanies = 9,
        FOBlocations = 10,
        ShippingAddress = 11,
        City = 12,
        State = 13,
        Zip = 14,
        CurrentBussinessCompanies = 15,
        BussinessInUSA = 16,
        AdditionalCriteriaForSupplier = 17,
        BenefitSupForFasilitate = 18,
        MediaPlatforms = 19,
        ThirdAttendeeName = 20,
        ThirdAttendeeTitle = 21,
        ThirdAttendeeCommEmail = 22,
        ThirdAttendeeCellPhone = 23,
        ThirdAttendeeWorkPhone = 24,
        ThirdAttendeeImage = 25
    }
}
