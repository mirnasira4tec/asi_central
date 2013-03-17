using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class DistributorAccountTypeMap : EntityTypeConfiguration<DistributorAccountType>
    {
        public DistributorAccountTypeMap()
        {
            // Primary Key
            this.ToTable("CENT_JoinASIAcctType_ACCT");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("CENT_AcctTypeID_ACCT");

            this.Property(t => t.Description)
                .HasColumnName("CENT_AcctTypeDesc_ACCT")
                .HasMaxLength(250);

            this.Property(t => t.SubCode)
                .HasColumnName("CENT_AcctType_SUBCODE")
                .HasMaxLength(24);

            this.Property(t => t.Deleted)
                .HasColumnName("CENT_Deleted_ACCT");

            this.Property(t => t.MemberTypeRole)
                .HasColumnName("CENT_MemberTypeRole_ACCT");
        }
    }
}
