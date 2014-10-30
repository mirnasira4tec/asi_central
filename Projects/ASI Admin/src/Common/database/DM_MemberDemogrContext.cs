using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using asi.asicentral.model.DM_memberDemogr;
namespace asi.asicentral.database
{
    public class DM_MemberDemogrContext : BaseContext
    {
        public DM_MemberDemogrContext(string connectionName)
            : base("name=" + connectionName)
        {
            Database.SetInitializer<DM_MemberDemogrContext>(null);
            EnableTracing(typeof(DM_MemberDemogrContext));
        }

        public DM_MemberDemogrContext()
            : this("DM_MemberDemogrContext")
        {
        }
        public DbSet<CompanyASIRep> CompanyASIReps { get; set; }
    }
}
