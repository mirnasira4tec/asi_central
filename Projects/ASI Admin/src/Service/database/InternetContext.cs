using asi.asicentral.database.mappings.asipublication;
using asi.asicentral.database.mappings.internet;
using asi.asicentral.model.counselor;
using asi.asicentral.model.news;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class InternetContext : BaseContext
    {
        public InternetContext()
            : base("name=InternetContext")
        {
            Database.SetInitializer<InternetContext>(null);
        }

        public DbSet<News> News { get; set; }
        public DbSet<NewsRotator> NewsRotators { get; set; }
        public DbSet<NewsSource> NewsSource { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
                .Add(new NewsMap())
                .Add(new NewsSourceMap())
                .Add(new NewsRotatorMap());
        }
    }
}
