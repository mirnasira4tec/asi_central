using asi.asicentral.database.mappings;
using asi.asicentral.model;
using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database
{
    public class ASIInternetContext : BaseContext
    {
        public ASIInternetContext()
            : base("name=ASIInternetContext")
        {
            Database.SetInitializer<ASIInternetContext>(null);
            // enable sql tracing
            this.EnableTracing(typeof(ASIInternetContext));
        }

        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationIssue> PublicationIssues { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PublicationConfiguration());
            modelBuilder.Configurations.Add(new PublicationIssueConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
