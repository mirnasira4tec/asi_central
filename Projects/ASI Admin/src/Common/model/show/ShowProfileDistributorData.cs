using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowProfileDistributorData
    {
       public int Id { get; set; }
       public int ProfileRequestId { get; set; }
       public string Email { get; set; }
       public string CompanyName { get; set; }
       public string ASINumber { get; set; }
       public string AttendeeName { get; set; }
       public string AttendeeTitle { get; set; }
       public string AttendeeCommEmail { get; set; }
       public string AttendeeCellPhone { get; set; }
       public string AttendeeWorkPhone { get; set; }
       public string AttendeeBiography { get; set; }
       public string Focus2018 { get; set; }
       public string BussinessFrom { get; set; }
       public string SalesByCustomer { get; set; }
       public string AnnualSalesVolume { get; set; }
       public decimal CatalogPercentage { get; set; }
       public decimal WebPercentage { get; set; }
       public decimal SpotPercentage { get; set; }
       public string DifferncFromOtherDistributor { get; set; }
       public bool HasSupplierNetwork { get; set; }
       public string VendorContact { get; set; }
       public bool PreviousBuyerEventAttendee { get; set; }
       public string BuyingGroupsDetail { get; set; }
       public bool PreviousFasilitateAttendee { get; set; }
       public string FasilitateAttendedDetail { get; set; }
       public bool IsBuyingGroup { get; set; }
       public string ShowSample { get; set; }
       public string SalesAids { get; set; }
       public string SellingMode { get; set; }
       public string SalesChallenge { get; set; }
       public string IdealSupDescription { get; set; }
       public string SupImportanceRating { get; set; }
       public string Importancelist { get; set; }
       public string CorporateAddress { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string Zip { get; set; }
       public string CompanyDescription { get; set; }
       public decimal CompanyAmtForProductSale { get; set; }
       public bool AcceptTerms { get; set; }
       public DateTime CreateDate { get; set; }
       public DateTime UpdateDate { get; set; }
       public string UpdateSource { get; set; }
       public bool IsUpdate { get; set; }
       public string AttendeeImage { get; set; }
       public string ClientLogo { get; set; }
       public virtual ShowProfileRequests ProfileRequests { get; set; }
       
    }
}
