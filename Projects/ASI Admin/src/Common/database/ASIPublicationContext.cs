using asi.asicentral.database.mappings.asipublication;
using asi.asicentral.model.counselor;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class ASIPublicationContext : BaseContext
    {
        public ASIPublicationContext()
            : base("name=ASIPublicationContext")
        {
            Database.SetInitializer<ASIPublicationContext>(null);
        }

        public DbSet<CounselorCategory> Categories { get; set; }
        public DbSet<CounselorContent> Contents { get; set; }
        public DbSet<CounselorFeature> Features { get; set; }
        public DbSet<CounselorFeatureRotator> FeatureRotators { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
               .Add(new LegacyCategoryMap())
               .Add(new LegacyContentMap())
               .Add(new LegacyFeatureContentMap())
               .Add(new LegacyFeatureContentRotatorMap());
        }
    }
}
