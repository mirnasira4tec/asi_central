using asi.asicentral.database.mappings.asipublication;
using asi.asicentral.database.mappings.internet;
using asi.asicentral.model.counselor;
using asi.asicentral.model.news;
using asi.asicentral.model.product;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class ProductContext : BaseContext
    {
        public ProductContext()
            : base("name=ProductContext")
        {
            Database.SetInitializer<ProductContext>(null);
        }

        public DbSet<Context> Contexts { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
                .Add(new ContextMap());
        }
    }
}
