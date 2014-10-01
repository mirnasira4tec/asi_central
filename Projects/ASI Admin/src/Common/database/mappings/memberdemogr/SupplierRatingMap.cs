using asi.asicentral.model.findsupplier;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.memberdemogr
{
    public class SupplierRatingMap : EntityTypeConfiguration<SupplierRating>
    {
        public SupplierRatingMap()
        {
            this.ToTable("MBDM_SPLR_Phone_SPHN");
            this.HasKey(t => t.SupplierId);

            // Properties
            this.Property(t => t.SupplierId)
                .HasColumnName("SRTG_SPLR_SUPPID");

            this.Property(t => t.ASINumber)
                .HasColumnName("SRTG_ASINum");

            this.Property(t => t.Overall)
                .HasColumnName("SRTG_Overall");

            this.Property(t => t.OverallDist)
                .HasColumnName("SRTG_OverallDist");

            this.Property(t => t.OverallTran)
                .HasColumnName("SRTG_OverallTran");

            this.Property(t => t.Quality)
                .HasColumnName("SRTG_Quality");

            this.Property(t => t.QualityDist)
                .HasColumnName("SRTG_QualityDist");

            this.Property(t => t.QualityTran)
                .HasColumnName("SRTG_QualityTran");

            this.Property(t => t.Communication)
                .HasColumnName("SRTG_Communication");

            this.Property(t => t.CommunicationDist)
                .HasColumnName("SRTG_CommunicationDist");

            this.Property(t => t.CommunicationTran)
                .HasColumnName("SRTG_CommunicationTran");

            this.Property(t => t.Delivery)
                .HasColumnName("SRTG_Delivery");

            this.Property(t => t.DeliveryDist)
                .HasColumnName("SRTG_DeliveryDist");

            this.Property(t => t.DeliveryTran)
                .HasColumnName("SRTG_DeliveryTran");

            this.Property(t => t.ProblemResolution)
                .HasColumnName("SRTG_ProblemResolution");

            this.Property(t => t.ProblemResolutionDist)
                .HasColumnName("SRTG_ProblemResolutionDist");

            this.Property(t => t.ProblemResolutionTran)
                .HasColumnName("SRTG_ProblemResolutionTran");

            this.Property(t => t.Imprint)
                .HasColumnName("SRTG_Imprint");

            this.Property(t => t.ImprintDist)
                .HasColumnName("SRTG_ImprintDist");

            this.Property(t => t.ImprintTran)
                .HasColumnName("SRTG_ImprintTran");

            this.Property(t => t.RateAvgCount)
                .HasColumnName("SRTG_RateAvgCount");

            this.Property(t => t.RateCount)
                .HasColumnName("SRTG_RateCount");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDate");

            this.Property(t => t.CreateSource)
                .HasColumnName("CreateSource");

        }
    }
}
