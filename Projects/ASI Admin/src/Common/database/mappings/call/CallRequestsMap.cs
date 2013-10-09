using asi.asicentral.model.call;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.call
{
    public class CallRequestsMap : EntityTypeConfiguration<CallRequest>
    {
        public CallRequestsMap()
        {
            // Table & Column Mappings
            this.ToTable("Call_Requests");
            // Primary Key

            this.HasKey(t => t.Requests_ID);

            // Properties
            this.Property(t => t.Req_Sec_Honeypot)
                .HasColumnName("Req_Sec_Honeypot")
                .HasMaxLength(250);

            this.Property(t => t.Req_Ent_ASINum)
                .HasColumnName("Req_Ent_ASINum")
                .HasMaxLength(12);

            this.Property(t => t.Req_Ent_Name)
                .HasColumnName("Req_Ent_Name")
                .HasMaxLength(250);

            this.Property(t => t.CreateSource)
                .HasColumnName("CreateSource")
                .HasMaxLength(128);

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource")
                .HasMaxLength(128);

            this.Property(t => t.AuditStatus_CD)
                .HasColumnName("AuditStatus_CD")
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Requests_ID)
                .HasColumnName("Requests_ID");

            this.Property(t => t.Req_IP)
                .HasColumnName("Req_IP");

            this.Property(t => t.Req_CallbackNumber)
                .HasColumnName("Req_CallbackNumber");

            this.Property(t => t.Req_CallbackTime)
                .HasColumnName("Req_CallbackTime");

            this.Property(t => t.Req_Status)
                .HasColumnName("Req_Status");

            this.Property(t => t.Req_Sec_Cookie)
                .HasColumnName("Req_Sec_Cookie");

            this.Property(t => t.Req_Sec_Queue)
                .HasColumnName("Req_Sec_Queue");

            this.Property(t => t.Req_Sec_JavaScript)
                .HasColumnName("Req_Sec_JavaScript");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDate");

            this.Property(t => t.Audit_XML)
                .HasColumnName("Audit_XML");

            this.Property(t => t.ROW_ID)
                .HasColumnName("ROW_ID");

            this.Property(t => t.MOD_ID)
                .HasColumnName("MOD_ID");

            this.Property(t => t.Req_Queue)
                .HasColumnName("Req_Queue");

            //relationship
        }
    }
}
