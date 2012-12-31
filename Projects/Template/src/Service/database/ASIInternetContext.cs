using asi.asicentral.database.mappings;
using asi.asicentral.model;
using asi.asicentral.services.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database
{
    public class ASIInternetContext : DbContext, IValidatedContext
    {
        public ASIInternetContext()
            : base("name=ASIInternetContext")
        {
            //nothing to be done
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

        public void Supports(Type type)
        {
            if (type != typeof(Publication) && type != typeof(PublicationIssue))
            {
                throw new Exception("Invalid context for the class: " + type.FullName);
            }
        }

        public DbSet GetSet(Type type)
        {
            if (type == typeof(Publication))
            {
                return Publications;
            }
            else if (type == typeof(PublicationIssue))
            {
                return PublicationIssues;
            }
            throw new Exception("Incompatible class for this context");
        }
    }
}
