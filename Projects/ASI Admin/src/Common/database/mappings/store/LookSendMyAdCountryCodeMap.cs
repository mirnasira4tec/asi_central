using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace asi.asicentral.database.mappings.store
{

    public class LookSendMyAdCountryCodeMap : EntityTypeConfiguration<LookSendMyAdCountryCode>
    {

        public LookSendMyAdCountryCodeMap()
        {
            this.ToTable("Look_SendMyAdCountryCode");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
