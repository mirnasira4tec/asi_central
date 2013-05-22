using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class LegacyTIMSSAdditionalInfoMap : EntityTypeConfiguration<TIMSSAdditionalInfo>
    {
        public LegacyTIMSSAdditionalInfoMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_ADDITIONAL_INFO");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("RecId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.NumberOfEmployees)
                .HasColumnName("DAPP_NoEmp");

            this.Property(t => t.NumberOfSalesPeople)
                .HasColumnName("DAPP_NoSalesPeople");

            this.Property(t => t.AnnualSalesVol)
                .HasColumnName("DAPP_AnnSalesVol");

            this.Property(t => t.ASIContact)
                .HasColumnName("DAPP_ASIContact")
                .HasMaxLength(150);

            this.Property(t => t.AnnualSalesVolumeASP)
                .HasColumnName("DAPP_AnnSalesVolASP");

            this.Property(t => t.BusinessRevenue)
                .HasColumnName("DAPP_BusinessRev")
                .HasMaxLength(50);

            this.Property(t => t.BusinessRevenueOther)
                .HasColumnName("DAPP_BusinessRevOther")
                .HasMaxLength(50);

            this.Property(t => t.IPAddress)
                .HasColumnName("DAPP_IPAdd")
                .HasMaxLength(25);

            this.Property(t => t.LoadStatus)
                .HasColumnName("LoadStatus")
                .HasMaxLength(20);

            this.Property(t => t.LoadDate)
                .HasColumnName("LoadDate");

            this.Property(t => t.YearEstablished)
                .HasColumnName("YEAR_ESTABLISHED");

            this.Property(t => t.Imprinter)
                .HasColumnName("IMPRINTER")
                .HasMaxLength(1);

            this.Property(t => t.Importer)
                .HasColumnName("IMPORTER")
                .HasMaxLength(1);

            this.Property(t => t.Manufacturer)
                .HasColumnName("MANUFACTURER")
                .HasMaxLength(1);

            this.Property(t => t.Retailer)
                .HasColumnName("RETAILER")
                .HasMaxLength(1);

            this.Property(t => t.Wholesaler)
                .HasColumnName("WHOLESALER")
                .HasMaxLength(1);

            this.Property(t => t.Etching)
                .HasColumnName("ETCHING")
                .HasMaxLength(1);

            this.Property(t => t.HotStamping)
                .HasColumnName("HOT_STAMPING")
                .HasMaxLength(1);

            this.Property(t => t.SilkScreen)
                .HasColumnName("SILKSCREEN")
                .HasMaxLength(1);

            this.Property(t => t.PadPrinting)
                .HasColumnName("PAD_PRINT")
                .HasMaxLength(1);

            this.Property(t => t.DirectEmbroidery)
                .HasColumnName("DIRECT_EMBROIDERY")
                .HasMaxLength(1);

            this.Property(t => t.FoilStamping)
                .HasColumnName("FOIL_STAMPING")
                .HasMaxLength(1);

            this.Property(t => t.Lithography)
                .HasColumnName("LITHOGRAPHY")
                .HasMaxLength(1);

            this.Property(t => t.Sublimation)
                .HasColumnName("SUBLIMATION")
                .HasMaxLength(1);

            this.Property(t => t.FourColorProcess)
                .HasColumnName("FOUR_COLOR_PROCESS")
                .HasMaxLength(1);

            this.Property(t => t.Engraving)
                .HasColumnName("ENGRAVING")
                .HasMaxLength(1);

            this.Property(t => t.Laser)
                .HasColumnName("LASER")
                .HasMaxLength(1);

            this.Property(t => t.Offset)
                .HasColumnName("OFFSET")
                .HasMaxLength(1);

            this.Property(t => t.Transfer)
                .HasColumnName("TRANSFER")
                .HasMaxLength(1);

            this.Property(t => t.FullColorProcess)
                .HasColumnName("FULL_COLOR_PROCESS")
                .HasMaxLength(1);

            this.Property(t => t.DieStamp)
                .HasColumnName("DIE_STAMP")
                .HasMaxLength(1);

            this.Property(t => t.ImprintOther)
                .HasColumnName("IMPRINT_OTHER")
                .HasMaxLength(1);

            this.Property(t => t.YearEstablishedAsAdSpecialist)
                .HasColumnName("YEAR_ESTAB_IN_AD_SPEC");

            this.Property(t => t.WomanOwned)
                .HasColumnName("WOMAN_OWNED")
                .HasMaxLength(100);

            this.Property(t => t.MinorityOwned)
                .HasColumnName("MINORITY_OWNED")
                .HasMaxLength(1);

            this.Property(t => t.HasAmericanProducts)
                .HasColumnName("AMER_MADE_AVAIL")
                .HasMaxLength(3);

            this.Property(t => t.BusinessHours)
                .HasColumnName("BUSINESS_HOURS")
                .HasMaxLength(60);

            this.Property(t => t.ProductionTime)
                .HasColumnName("PROD_TIME_MIN");

            this.Property(t => t.RushService)
                .HasColumnName("RUSH_SERVICE")
                .HasMaxLength(3);

            this.Property(t => t.LineName)
                .HasColumnName("LineName")
                .HasMaxLength(50);
        }
    }
}
