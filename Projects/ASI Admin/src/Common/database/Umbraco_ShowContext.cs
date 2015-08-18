using asi.asicentral.database.mappings.show;
using asi.asicentral.model.show;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class Umbraco_ShowContext : BaseContext
    {
        public Umbraco_ShowContext()
            : base("name=Umbraco_ShowContext")
        {
            Database.SetInitializer<Umbraco_ShowContext>(null);
            EnableTracing(typeof(Umbraco_ShowContext));
        }
        public DbSet<ShowAddress> Address { get; set; }
        public DbSet<ShowAttendee> Attendee { get; set; }
        public DbSet<ShowEmployee> Employee { get; set; }
        public DbSet<ShowEmployeeAttendee> EmployeeAttendee { get; set; }
        public DbSet<ShowASI> Show { get; set; }
        public DbSet<ShowCompany> Company { get; set; }
        public DbSet<ShowType> ShowType { get; set; }
        public DbSet<ShowCompanyAddress> CompanyAddress { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Configuration.LazyLoadingEnabled = true;
            modelBuilder.Configurations
                .Add(new ShowAddressMap())
                .Add(new ShowAttendeeMap())
                .Add(new ShowCompanyMap())
                .Add(new ShowEmployeeAttendeeMap())
                .Add(new ShowEmployeeMap())
                .Add(new ShowMap())
                .Add(new ShowTypeMap())
                .Add(new ShowCompanyAddressMap());
    }
}
}