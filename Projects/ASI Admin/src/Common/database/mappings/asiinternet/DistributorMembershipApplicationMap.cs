using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class DistributorMembershipApplicationMap : EntityTypeConfiguration<DistributorMembershipApplication>
    {
        public DistributorMembershipApplicationMap()
        {
            this.ToTable("CENT_DistJoinApp_DAPP");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("DAPP_AppID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.ApplicationStatusId)
                .HasColumnName("STAT_AppStatusID");

            this.Property(t => t.Company)
                .HasColumnName("DAPP_Company")
                .HasMaxLength(100);

            this.Property(t => t.BillingAddress1)
                .HasColumnName("DAPP_Street1")
                .HasMaxLength(150);

            this.Property(t => t.BillingAddress2)
                .HasColumnName("DAPP_Street2")
                .HasMaxLength(150);

            this.Property(t => t.BillingCity)
                .HasColumnName("DAPP_City")
                .HasMaxLength(75);

            this.Property(t => t.BillingState)
                .HasColumnName("DAPP_StateID")
                .HasMaxLength(15);

            this.Property(t => t.BillingZip)
                .HasColumnName("DAPP_Zip")
                .HasMaxLength(15);

            this.Property(t => t.BillingCountry)
                .HasColumnName("DAPP_Country")
                .HasMaxLength(100);

            this.Property(t => t.BillingPhone)
                .HasColumnName("DAPP_Phone")
                .HasMaxLength(35);

            this.Property(t => t.BillingFax)
                .HasColumnName("DAPP_Fax")
                .HasMaxLength(35);

            this.Property(t => t.BillingEmail)
                .HasColumnName("DAPP_Email")
                .HasMaxLength(256);

            this.Property(t => t.BillingWebUrl)
                .HasColumnName("DAPP_WebAdd")
                .HasMaxLength(256);

            this.Property(t => t.FirstName)
                .HasColumnName("DAPP_FirstName")
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .HasColumnName("DAPP_LastName")
                .HasMaxLength(100);

            this.Property(t => t.ShippingStreet1)
                .HasColumnName("DAPP_ShipStreet1")
                .HasMaxLength(150);

            this.Property(t => t.ShippingStreet2)
                .HasColumnName("DAPP_ShipStreet2")
                .HasMaxLength(150);

            this.Property(t => t.ShippingCity)
                .HasColumnName("DAPP_ShipCity")
                .HasMaxLength(75);

            this.Property(t => t.ShippingState)
                .HasColumnName("DAPP_ShipState")
                .HasMaxLength(15);

            this.Property(t => t.ShippingZip)
                .HasColumnName("DAPP_ShipZip")
                .HasMaxLength(15);

            this.Property(t => t.ShippingCountry)
                .HasColumnName("DAPP_ShipCountry")
                .HasMaxLength(100);

            this.Property(t => t.NumberOfEmployee)
                .HasColumnName("DAPP_NoEmp");

            this.Property(t => t.NumberOfSalesEmployee)
                .HasColumnName("DAPP_NoSalesPeople");

            this.Property(t => t.AnnualSalesVolume)
                .HasColumnName("DAPP_AnnSalesVol")
                .HasMaxLength(50);

            this.Property(t => t.ASIContact)
                .HasColumnName("DAPP_ASIContact")
                .HasMaxLength(150);

            this.Property(t => t.AnnualSalesVolumeASP)
                .HasColumnName("DAPP_AnnSalesVolASP")
                .HasMaxLength(50);

            this.Property(t => t.CorporateOfficer)
                .HasColumnName("DAPP_CorpOfficer");

            this.Property(t => t.SignatureType)
                .HasColumnName("SIGT_SigType");

            this.Property(t => t.IsMajorForResale)
                .HasColumnName("DAPP_MajResale");

            this.Property(t => t.IsForProfit)
                .HasColumnName("DAPP_ForProfit");

            this.Property(t => t.ProvideInvoiceOnDemand)
                .HasColumnName("DAPP_NoInv");

            this.Property(t => t.IsSolelyWork)
                .HasColumnName("DAPP_SolelyWork");

            this.Property(t => t.SolelyWorkName)
                .HasColumnName("DAPP_SolelyWorkName")
                .HasMaxLength(250);

            this.Property(t => t.InformASIOfChange)
                .HasColumnName("DAPP_InformASI");

            this.Property(t => t.ApplicantName)
                .HasColumnName("DAPP_AppName")
                .HasMaxLength(150);

            this.Property(t => t.ApplicantEmail)
                .HasColumnName("DAPP_AppEmail")
                .HasMaxLength(150);

            this.Property(t => t.TrueAnswers)
                .HasColumnName("DAPP_TrueAnswers");

            this.Property(t => t.AgreeReceivePromotionalProducts)
                .HasColumnName("DAPP_RecSpecials");

            this.Property(t => t.AgreeTermsAndConditions)
                .HasColumnName("DAPP_TermsCond");

            this.Property(t => t.IsMajorityDistributeForResale)
                .HasColumnName("DAPP_DistResale");

            this.Property(t => t.IPAddress)
                .HasColumnName("DAPP_IPAdd")
                .HasMaxLength(25);

            this.Property(t => t.Custom1)
                .HasColumnName("DAPP_Custom1")
                .HasMaxLength(500);

            this.Property(t => t.Custom2)
                .HasColumnName("DAPP_Custom2")
                .HasMaxLength(500);

            this.Property(t => t.Custom3)
                .HasColumnName("DAPP_Custom3")
                .HasMaxLength(500);

            this.Property(t => t.Custom4)
                .HasColumnName("DAPP_Custom4")
                .HasMaxLength(500);

            this.Property(t => t.Custom5)
                .HasColumnName("DAPP_Custom5")
                .HasMaxLength(500);

            this.Property(t => t.PrimaryBusinessRevenueId)
                .HasColumnName("DAPP_BusinessRevID");

            this.Property(t => t.OtherBusinessRevenue)
                .HasColumnName("DAPP_BusinessRevOther")
                .HasMaxLength(250);

            this.Property(t => t.AccountTypes)
                .HasColumnName("DAPP_AccountTypes")
                .HasMaxLength(500);

            this.Property(t => t.AccountTypes)
                .HasColumnName("DAPP_AccountTypes")
                .HasMaxLength(500);

            this.Property(t => t.ProductLines)
                .HasColumnName("DAPP_ProductLines")
                .HasMaxLength(500);

            this.Property(t => t.EstablishedDate)
                .HasColumnName("DAPP_EstablishedDate");

            this.Property(t => t.Address1)
                .HasColumnName("DAPP_ComAddress1");

            this.Property(t => t.Address2)
                .HasColumnName("DAPP_ComAddress2");

            this.Property(t => t.City)
                .HasColumnName("DAPP_ComCity");

            this.Property(t => t.Zip)
                .HasColumnName("DAPP_ComZip");

            this.Property(t => t.State)
                .HasColumnName("DAPP_ComState");

            this.Property(t => t.Country)
                .HasColumnName("DAPP_ComCountry");

            this.Property(t => t.Phone)
                .HasColumnName("DAPP_ComPhone");

            this.Property(t => t.InternationalPhone)
                .HasColumnName("DAPP_ComIntPhone");

            this.Property(t => t.HasShipAddress)
                .HasColumnName("DAPP_HasShipAddress");

            this.Property(t => t.HasBillAddress)
                .HasColumnName("DAPP_HasBillAddress");
        }
    }
}
