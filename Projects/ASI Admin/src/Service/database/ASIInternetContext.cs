using asi.asicentral.database.mappings;
using asi.asicentral.model.sgr;
using asi.asicentral.services.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new CompanyConfiguration());
        }

        #region IValidatedContext

        public void Supports(Type type)
        {
            if (!typeof(Company).IsAssignableFrom(type))
            {
                throw new Exception("Invalid context for the class: " + type.FullName);
            }
        }

        public DbSet GetSet(Type type)
        {
            if (typeof(Company).IsAssignableFrom(type))
            {
                return Companies;
            }
            throw new Exception("Incompatible class for this context");
        }

        #endregion IValidatedContext
    }
}
