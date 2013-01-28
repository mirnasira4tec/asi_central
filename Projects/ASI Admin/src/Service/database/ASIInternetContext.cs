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
            Database.SetInitializer<ASIInternetContext>(null);
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
               .Add(new CompanyConfiguration())
               .Add(new ProductConfiguration())
               .Add(new CategoryConfiguration());
        }

        #region IValidatedContext

        public void Supports(Type type)
        {
            if (!(typeof(Company).IsAssignableFrom(type) || typeof(Product).IsAssignableFrom(type) || typeof(Category).IsAssignableFrom(type)))
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
            else if (typeof(Product).IsAssignableFrom(type))
            {
                return Products;
            }
            else if (typeof(Category).IsAssignableFrom(type))
            {
                return Categories;
            }
            else
                throw new Exception("Incompatible class for this context");
        }

        #endregion IValidatedContext
    }
}
