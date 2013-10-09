using asi.asicentral.database.mappings.call;
using asi.asicentral.model.call;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database
{
    public class CallContext : BaseContext
    {
        public CallContext()
            : base("name=CallContext")
        {
            Database.SetInitializer<CallContext>(null);
            EnableTracing(typeof(CallContext));
        }

        public DbSet<CallQueue> Queues { get; set; }
        public DbSet<CallRequest> Requests { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
               .Add(new CallQueueMap())
               .Add(new CallRequestsMap());
        }
    }
}
