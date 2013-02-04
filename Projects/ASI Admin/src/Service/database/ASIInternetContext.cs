using asi.asicentral.database.mappings;
using asi.asicentral.model.sgr;
using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<asi.asicentral.model.store.Order> Orders { get; set; }
        public DbSet<asi.asicentral.model.store.OrderDetail> OrderDetails { get; set; }
        public DbSet<asi.asicentral.model.store.StoreProduct> StoreProducts { get; set; }

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
               .Add(new CategoryConfiguration())
               .Add(new OrderConfiguration())
               .Add(new OrderDetailConfiguration())
               .Add(new StoreProductConfiguration());
        }
    }
}
