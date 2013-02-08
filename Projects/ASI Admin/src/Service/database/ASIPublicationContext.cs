using asi.asicentral.database.mappings;
using asi.asicentral.model.sgr;
using System.Data.Entity;
using asi.asicentral.model.store;
using Publications.Models.Mapping;

namespace asi.asicentral.database
{
    public class ASIPublicationContext : BaseContext
    {
        public ASIPublicationContext()
            : base("name=ASIPublicationContext")
        {
            Database.SetInitializer<ASIPublicationContext>(null);
        }


        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
               .Add(new CounselorCategoryMap())
               .Add(new CounselorContentMap())
               .Add(new CounselorFeatureContentMap());
        }
    }
}
