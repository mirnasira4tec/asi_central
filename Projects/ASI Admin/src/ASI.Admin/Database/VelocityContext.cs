using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data.Entity.ModelConfiguration;
using asi.asicentral.web.Interface;
using System.Data;
using System.Linq;
using asi.asicentral.web.Models.velocity;

namespace asi.asicentral.web.database
{
    public class VelocityContext : DbContext, IVelocityContext
    {
        public VelocityContext()
            : base("name=VelocityContext")
        {
            Database.SetInitializer<VelocityContext>(null);
        }

        public virtual bool MapColor(ColorMapping colorMapping)
        {
            object[] param = 
            {
                new SqlParameter("@CompanyId", colorMapping.CompayId),
                new SqlParameter("@ColorGroup", colorMapping.ColorGroup ),
                new SqlParameter("@SupplierColor", colorMapping.SupplierColor ),                
            };

            var val = Database.SqlQuery<long>("SP_INSERTCOLORMAPPING @CompanyId, @ColorGroup, @SupplierColor", param)
                                .ToList<long>();
            return val[0] > 0;
        }
    }
}
