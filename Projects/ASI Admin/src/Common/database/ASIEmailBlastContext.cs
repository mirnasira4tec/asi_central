using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using asi.asicentral.model.store;
using asi.asicentral.database.mappings.asiemailblast;

namespace asi.asicentral.database
{

    public class ASIEmailBlastContext : BaseContext
    {

        public ASIEmailBlastContext(string connectionName)
            : base("name=" + connectionName)
        {
            Database.SetInitializer<ASIEmailBlastContext>(null);
        }

        public ASIEmailBlastContext()
            : this("ASIEmailBlastContext")
        {
        }

        public DbSet<ClosedCampaignDate> ClosedCampaignDates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Configuration.LazyLoadingEnabled = true;
            modelBuilder.Configurations
                .Add(new ClosedCampaignDateMap());
        }
    }
}
