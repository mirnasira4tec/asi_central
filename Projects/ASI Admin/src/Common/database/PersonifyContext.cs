using asi.asicentral.database.mappings.personify;
using asi.asicentral.model.personify;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class PersonifyContext : BaseContext
    {
        public PersonifyContext()
            : base("name=PersonifyMappingContext")
        {
            Database.SetInitializer<PersonifyContext>(null);      
        }

        public DbSet<PersonifyMapping> PersonifyProducts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
                .Add(new PersonifyMappingMap());
        }
    }
}
