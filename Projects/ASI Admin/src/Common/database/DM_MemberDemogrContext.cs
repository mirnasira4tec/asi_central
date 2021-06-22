using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using asi.asicentral.model.DM_memberDemogr;
using asi.asicentral.database.mappings.DM_memberDemogr;
namespace asi.asicentral.database
{
    public class DM_MemberDemogrContext : BaseContext
    {
        public DM_MemberDemogrContext(string connectionName)
            : base("name=" + connectionName)
        {
            Database.SetInitializer<DM_MemberDemogrContext>(null);
        }

        public DM_MemberDemogrContext()
            : this("DM_MemberDemogrContext")
        {
        }

        public DbSet<CompanyASIRep> CompanyASIReps { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
               .Add(new CompanyASIRepMap());
        }
    }
}
