using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace asi.asicentral.database.mappings.store
{
    public class StoreCompanyMap : EntityTypeConfiguration<StoreCompany>
    {
        public StoreCompanyMap()
        {
            this.ToTable("STOR_Company");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("CompanyId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasMany(t => t.Addresses)
                .WithOptional()
                .HasForeignKey(t => t.CompanyId)
                .WillCascadeOnDelete();
        }
    }
}
