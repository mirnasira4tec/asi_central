using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class DistributorProductLineMap : EntityTypeConfiguration<DistributorProductLine>
    {
        public DistributorProductLineMap()
        {
            this.ToTable("CENT_JoinASIProdType_PROD");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("CENT_ProdTypeID_PROD");

            this.Property(t => t.Description)
                .HasColumnName("CENT_ProdTypeID_PROD")
                .HasMaxLength(250);

            this.Property(t => t.Description)
                .HasColumnName("CENT_ProdTypeDesc_PROD");

            this.Property(t => t.MemberTypeRole)
                .HasColumnName("CENT_MemberTypeRole_PROD")
                .HasMaxLength(150);

            this.Property(t => t.SubCode)
                .HasColumnName("CENT_AcctType_SUBCODE")
                .HasMaxLength(24);

            this.Property(t => t.Deleted)
                .HasColumnName("CENT_Deleted_PROD");
        }
    }
}
