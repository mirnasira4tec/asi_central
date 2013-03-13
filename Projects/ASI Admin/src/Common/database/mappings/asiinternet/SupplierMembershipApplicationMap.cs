using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class SupplierMembershipApplicationMap : EntityTypeConfiguration<SupplierMembershipApplication>
    {
        public SupplierMembershipApplicationMap()
        {
            this.ToTable("CENT_SuppJoinApp_SAPP");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("SAPP_AppID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            this.Property(t => t.UserId)
                .HasColumnName("SAPP_UserID");

            this.Property(t => t.ApplicationStatusId)
                .HasColumnName("STAT_AppStatusID");

            this.Property(t => t.Company)
                .HasColumnName("SAPP_Company")
                .HasMaxLength(100);

            this.Property(t => t.BillingAddress1)
                .HasColumnName("SAPP_BillAddress")
                .HasMaxLength(150);

            this.Property(t => t.BillingAddress2)
                .HasColumnName("SAPP_BillAddress2")
                .HasMaxLength(100);

            this.Property(t => t.BillingCity)
                .HasColumnName("SAPP_BillCity")
                .HasMaxLength(75);

            this.Property(t => t.BillingState)
                .HasColumnName("SAPP_BillState")
                .HasMaxLength(50);

            this.Property(t => t.BillingZip)
                .HasColumnName("SAPP_BillZip")
                .HasMaxLength(15);

            this.Property(t => t.BillingCountry)
                .HasColumnName("SAPP_BillCountry")
                .HasMaxLength(100);

            this.Property(t => t.BillingPhone)
                .HasColumnName("SAPP_BillPhone")
                .HasMaxLength(35);

            this.Property(t => t.BillingTollFree)
                .HasColumnName("SAPP_BillTollFree")
                .HasMaxLength(35);

            this.Property(t => t.BillingFax)
                .HasColumnName("SAPP_BillFax")
                .HasMaxLength(35);

            this.Property(t => t.BillingWebUrl)
                .HasColumnName("SAPP_BillWebAdd")
                .HasMaxLength(256);

            this.Property(t => t.BillingEmail)
                .HasColumnName("SAPP_BillEmail")
                .HasMaxLength(256);

            this.Property(t => t.ShippingStreet1)
                .HasColumnName("SAPP_ShipAddress")
                .HasMaxLength(150);

            this.Property(t => t.ShippingStreet2)
                .HasColumnName("SAPP_ShipAddress2")
                .HasMaxLength(100);

            this.Property(t => t.ShippingCity)
                .HasColumnName("SAPP_ShipCity")
                .HasMaxLength(75);

            this.Property(t => t.ShippingState)
                .HasColumnName("SAPP_ShipState")
                .HasMaxLength(50);

            this.Property(t => t.ShippingZip)
                .HasColumnName("SAPP_ShipZip")
                .HasMaxLength(15);

            this.Property(t => t.ShippingCountry)
                .HasColumnName("SAPP_ShipCountry")
                .HasMaxLength(100);

            this.Property(t => t.ContactName)
                .HasColumnName("SAPP_PrimName")
                .HasMaxLength(100);

            this.Property(t => t.ContactTitle)
                .HasColumnName("SAPP_PrimTitle")
                .HasMaxLength(100);

            this.Property(t => t.ContactEmail)
                .HasColumnName("SAPP_PrimEmail")
                .HasMaxLength(256);

            this.Property(t => t.ContactPhone)
                .HasColumnName("SAPP_PrimPhone")
                .HasMaxLength(35);

            this.Property(t => t.LineNames)
                .HasColumnName("SAPP_LineNames")
                .HasMaxLength(500);

            this.Property(t => t.LineMinorityOwned)
                .HasColumnName("SAPP_Minority");

            this.Property(t => t.SalesVolume)
                .HasColumnName("SAPP_SalesVol")
                .HasMaxLength(50);

            this.Property(t => t.YearEstablished).
                HasColumnName("SAPP_YearEst");

            this.Property(t => t.YearEnteredAdvertising)
                .HasColumnName("SAPP_YearAdv");

            this.Property(t => t.OfficeHourStart)
                .HasColumnName("SAPP_OffHourStart")
                .HasMaxLength(35);

            this.Property(t => t.OfficeHourEnd)
                .HasColumnName("SAPP_OffHourEnd")
                .HasMaxLength(35);

            this.Property(t => t.NumberOfEmployee)
                .HasColumnName("SAPP_TotalEmployee")
                .HasMaxLength(35);

            this.Property(t => t.NumberOfSalesEmployee)
                .HasColumnName("SAPP_SalesForce")
                .HasMaxLength(30);

            this.Property(t => t.IsImprinterVsDecorator)
                .HasColumnName("SAPP_ImprintDec");

            this.Property(t => t.IsImporter)
                .HasColumnName("SAPP_Importer");

            this.Property(t => t.IsManufacturer)
                .HasColumnName("SAPP_Manufacturer");

            this.Property(t => t.IsRetailer)
                .HasColumnName("SAPP_Retailer");

            this.Property(t => t.IsWholesaler)
                .HasColumnName("SAPP_Wholesaler");

            this.Property(t => t.FedTaxId)
                .HasColumnName("SAPP_FedTaxID");

            this.Property(t => t.SellToEndUsers)
                .HasColumnName("SAPP_ThruEndUsers");

            this.Property(t => t.SellThruDistributors)
                .HasColumnName("SAPP_ThruDistributors");

            this.Property(t => t.SellThruInternet)
                .HasColumnName("SAPP_ThruInternet");

            this.Property(t => t.SellThruDirectMarketing)
                .HasColumnName("SAPP_ThruDirect");

            this.Property(t => t.SellThruRetail)
                .HasColumnName("SAPP_ThruRetail");

            this.Property(t => t.SellThruAffiliate)
                .HasColumnName("SAPP_ThruAff");

            this.Property(t => t.AffiliateCompanyName)
                .HasColumnName("SAPP_AffCompany")
                .HasMaxLength(100);

            this.Property(t => t.AffiliateASINumber)
                .HasColumnName("SAPP_AffASI");

            this.Property(t => t.IsUnionMade)
                .HasColumnName("SAPP_UnionMade");

            this.Property(t => t.ProductionTime)
                .HasColumnName("SAPP_ProdTime")
                .HasMaxLength(35);

            this.Property(t => t.IsRushServiceAvailable)
                .HasColumnName("SAPP_Rush");

            this.Property(t => t.OtherDec)
                .HasColumnName("SAPP_OtherDec")
                .HasMaxLength(200);

            this.Property(t => t.IsUPSAvailable).
                HasColumnName("SAPP_UPS");

            this.Property(t => t.UPSAddress)
                .HasColumnName("SAPP_UpsAddress")
                .HasMaxLength(50);

            this.Property(t => t.UPSCity)
                .HasColumnName("SAPP_UpsCity")
                .HasMaxLength(50);

            this.Property(t => t.UPSState)
                .HasColumnName("SAPP_UpsState")
                .HasMaxLength(50);

            this.Property(t => t.UPSZip)
                .HasColumnName("SAPP_UpsZip")
                .HasMaxLength(50);

            this.Property(t => t.UPSShippingNumber)
                .HasColumnName("SAPP_UpsShipNum")
                .HasMaxLength(50);

            this.Property(t => t.AuthorizeUPSNewAccount)
                .HasColumnName("SAPP_UpsAuth");

            this.Property(t => t.AgreeUPSTermsAndConditions)
                .HasColumnName("SAPP_UpsTerms");

            this.Property(t => t.AgreeASITermsAndConditions)
                .HasColumnName("SAPP_AsiTerms");

            this.Property(t => t.WomanOwned)
                .HasColumnName("SAPP_WomanOwned");

            this.Property(t => t.Address1)
                .HasColumnName("SAPP_ComAddress1");

            this.Property(t => t.Address2)
                .HasColumnName("SAPP_ComAddress2");

            this.Property(t => t.City)
                .HasColumnName("SAPP_ComCity");

            this.Property(t => t.Zip)
                .HasColumnName("SAPP_ComZip");

            this.Property(t => t.State)
                .HasColumnName("SAPP_ComState")
                .HasMaxLength(50);

            this.Property(t => t.Country)
                .HasColumnName("SAPP_ComCountry");

            this.Property(t => t.Phone)
                .HasColumnName("SAPP_ComPhone");

            this.Property(t => t.InternationalPhone)
                .HasColumnName("SAPP_ComIntPhone");

            this.Property(t => t.HasShipAddress)
                .HasColumnName("SAPP_HasShipAddress");

            this.Property(t => t.HasBillAddress)
                .HasColumnName("SAPP_HasBillAddress");
        }
    }
}
