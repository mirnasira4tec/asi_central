using asi.asicentral.database.mappings.show;
using asi.asicentral.model.show;
using System.Data.Entity;


namespace asi.asicentral.database
{
    public class ShowContext : BaseContext
    {
        public ShowContext()
            : base("name=ShowContext")
        {
            Database.SetInitializer<ShowContext>(null);
            EnableTracing(typeof(ShowContext));
        }
        public DbSet<Address> Address { get; set; }
        public DbSet<Attendee> Attendee { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeAttendee> EmployeeAttendee { get; set; }
        public DbSet<Show> Show { get; set; }
        public DbSet<ShowCompany> Company { get; set; }
        public DbSet<ShowType> ShowType { get; set; }
        public DbSet<CompanyAddress> CompanyAddress { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Configuration.LazyLoadingEnabled = true;
            modelBuilder.Configurations
                .Add(new AddressMap())
                .Add(new AttendeeMap())
                .Add(new CompanyMap())
                .Add(new EmployeeAttendeeMap())
                .Add(new EmployeeMap())
                .Add(new ShowMap())
                .Add(new ShowTypeMap())
                .Add(new CompanyAddressMap());
        }
    }
}
