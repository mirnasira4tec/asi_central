using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class CENTUserProfilesPROFMap : EntityTypeConfiguration<CENTUserProfilesPROF>
    {
        public CENTUserProfilesPROFMap()
        {
            ToTable("CENT_UserProfiles_PROF");
            HasKey(prof => prof.PROF_UserID);
        }
    }
}
