using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailEquipmentMembership : StoreDetailApplication
    {
        public static int[] Identifiers = new int[] { 84, 85, 86, 87, 88};

        public StoreDetailEquipmentMembership()
        {
            if (this.GetType() == typeof(StoreDetailEquipmentMembership))
            {
                EquipmentTypes = new List<LookEquipmentType>();
            }
        }

        [Display(ResourceType = typeof(Resource), Name = "MinorityOwned")]
        public Nullable<bool> IsMinorityOwned { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SalesVolume")]
        public string SalesVolume { get; set; }

        [Range(1700, 2050, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldYearRange")]
        [Display(ResourceType = typeof(Resource), Name = "YearEstablished")]
        public Nullable<int> YearEstablished { get; set; }

        [Range(1700, 2050, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldYearRange")]
        [Display(ResourceType = typeof(Resource), Name = "YearEnteredAdvertising")]
        public Nullable<int> YearEnteredAdvertising { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OfficeHour")]
        public string OfficeHourStart { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OfficeHourEnd")]
        public string OfficeHourEnd { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployee")]
        public string NumberOfEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployeesLocatedAtBilling")]
        public string NumberOfLocatedAtBilling { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployeesNotInNorthAmerica")]
        public string NumberOfEmployeesNotInNorthAmerica { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsEmployeesSharedWithOther")]
        public Nullable<bool> IsEmployeesSharedWithOther { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "StoreCompanyName")]
        public string CompanyName1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "StoreCompanyName")]
        public string CompanyName2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "StoreCompanyName")]
        public string CompanyName3 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ASI")]
        public string ASINumber1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ASI")]
        public string ASINumber2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ASI")]
        public string ASINumber3 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OtherDec")]
        public string OtherDec { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "TotalSalesForce")]
        public string TotalSalesForce { get; set; }

        public virtual IList<LookEquipmentType> EquipmentTypes { get; set; }

        public override string ToString()
        {
            return "Equipment Membership " + OrderDetailId;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreDetailEquipmentMembership equipment = obj as StoreDetailEquipmentMembership;
            if (equipment != null) equals = equipment.OrderDetailId == OrderDetailId;
            return equals;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + "StoreDetailEquipmentMembership".GetHashCode();
            hash = hash * 31 + OrderDetailId.GetHashCode();
            return hash;
        }

        public virtual void CopyTo(StoreDetailEquipmentMembership equipment)
        {
            base.CopyTo(equipment);
            equipment.IsMinorityOwned = IsMinorityOwned;
            equipment.SalesVolume = SalesVolume;
            equipment.YearEstablished = YearEstablished;
            equipment.YearEnteredAdvertising = YearEnteredAdvertising;
            equipment.OfficeHourEnd = OfficeHourEnd;
            equipment.OfficeHourStart = OfficeHourStart;
            equipment.NumberOfEmployee = NumberOfEmployee;
            equipment.NumberOfLocatedAtBilling = NumberOfLocatedAtBilling;
            equipment.NumberOfEmployeesNotInNorthAmerica = NumberOfEmployeesNotInNorthAmerica;
            equipment.IsEmployeesSharedWithOther = IsEmployeesSharedWithOther;
            if (equipment.IsEmployeesSharedWithOther != null && equipment.IsEmployeesSharedWithOther.Value)
            {
                equipment.CompanyName1 = CompanyName1;
                equipment.CompanyName2 = CompanyName2;
                equipment.CompanyName3 = CompanyName3;
                equipment.ASINumber1 = ASINumber1;
                equipment.ASINumber2 = ASINumber2;
                equipment.ASINumber3 = ASINumber3;
            }
            else
            {
                equipment.CompanyName1 = null;
                equipment.CompanyName2 = null;
                equipment.CompanyName3 = null;
                equipment.ASINumber1 = null;
                equipment.ASINumber2 = null;
                equipment.ASINumber3 = null;
            }
            equipment.TotalSalesForce = TotalSalesForce;
            equipment.OtherDec = OtherDec;
            equipment.EquipmentTypes = EquipmentTypes;
        }
    }
}
