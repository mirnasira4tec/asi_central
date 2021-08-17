using asi.asicentral.database.mappings.show;
using asi.asicentral.database.mappings.show.form;
using asi.asicentral.model.show;
using asi.asicentral.model.show.form;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class Umbraco_ShowContext : BaseContext
    {
        public Umbraco_ShowContext()
            : this("Umbraco_ShowContext")
        {
        }
        public Umbraco_ShowContext(string connectionName)
            : base("name=" + connectionName)
        {
            Database.SetInitializer<Umbraco_ShowContext>(null);
        }
        public DbSet<ShowAddress> Address { get; set; }
        public DbSet<ShowAttendee> Attendee { get; set; }
        public DbSet<ShowEmployee> Employee { get; set; }
        public DbSet<ShowEmployeeAttendee> EmployeeAttendee { get; set; }
        public DbSet<ShowASI> Show { get; set; }
        public DbSet<ShowCompany> Company { get; set; }
        public DbSet<ShowType> ShowType { get; set; }
        public DbSet<ShowCompanyAddress> CompanyAddress { get; set; }
        public DbSet<ShowDistShowLogo> DistShowLogo { get; set; }
        public DbSet<ShowProfileSupplierData> ProfileSupplierData { get; set; }
        public DbSet<ShowProfileOptionalDataLabel> ProfileOptionalDataLabel { get; set; }
        public DbSet<ShowProfileRequests> ProfileRequests { get; set; }
        public DbSet<ShowProfileOptionalDetails> ProfileOptionalDetails { get; set; }
        public DbSet<ShowProfileDistributorData> ProfileDistributorData { get; set; }
        public DbSet<ShowFormType> ShowFormType { get; set; }
        public DbSet<ShowFormInstance> ShowFormInstance { get; set; }
        public DbSet<ShowFormPropertyValue> ShowFormPropertyValue { get; set; }
        public DbSet<SHW_FormType> SHW_ShowFormType { get; set; }
        public DbSet<SHW_FormInstance> SHW_FormInstance { get; set; }
        public DbSet<SHW_FormPropertyValue> SHW_ShowFormPropertyValue { get; set; }
        public DbSet<SHW_ShowFormInstance> SHW_ShowFormInstance { get; set; }
        public DbSet<SHW_FormQuestion> SHW_FormQuestion { get; set; }
        public DbSet<SHW_FormQuestionOption> SHW_FormQuestionOption { get; set; }

        public DbSet<ProfilePackage> ProfilePackages { get; set; }
        public DbSet<ProfileOption> ProfileOptions { get; set; }
        public DbSet<ProfileOptionValue> ProfileOptionValue { get; set; }
        public DbSet<ProfilePackageOption> ProfilePackageOptions { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<CompanyProfileData> CompanyProfileData { get; set; }

        public DbSet<ShowSchedule> ShowSchedules { get; set; }
        public DbSet<ShowScheduleDetail> ShowScheduleDetails { get; set; }
        public DbSet<AttendeeSchedule> AttendeeSchedules { get; set; }

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
                .Add(new ShowCompanyAddressMap())
                .Add(new ShowDistShowLogoMap())
                .Add(new ShowProfileSupplierDataMap())
                .Add(new ShowProfileOptionalDataLabelMap())
                .Add(new ShowProfileRequestMap())
                .Add(new ShowProfileOptionalDetailsMap())
                .Add(new ShowProfileDistributorDataMap())
                .Add(new ShowFormTypeMap())
                .Add(new ShowFormInstanceMap())
                .Add(new ShowFormValueMap())
                .Add(new ProfilePackageMap())
                .Add(new ProfileOptionMap())
                .Add(new ProfileOptionValueMap())
                .Add(new ProfilePackageOptionMap())
                .Add(new CompanyProfileMap())
                .Add(new CompanyProfileDataMap())
                .Add(new SHW_FormTypeMap())
                .Add(new SHW_FormInstanceMap())
                .Add(new SHW_FormPropertyValueMap())
                .Add(new SHW_ShowFormInstanceMap())
                .Add(new SHW_FormQuestionMap())
                .Add(new SHW_FormQuestionOptionMap())
                .Add(new ShowScheduleMap())
                .Add(new ShowScheduleDetailMap())
                .Add(new AttendeeScheduleMap());
        }
    }
}