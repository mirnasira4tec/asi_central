using asi.asicentral.interfaces;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util.store.magazinesadvertising
{
    public static class Update
    {

        public static void UpdateDateUTCAndSource<T>(this T o, [CallerMemberName] string source = "") where T : IDateUTCAndSource
        {
            if (o.CreateDate == null || o.CreateDate == default(DateTime)) o.CreateDate = DateTime.UtcNow;
            o.UpdateDate = DateTime.UtcNow;
            o.UpdateSource = string.Format("{0} - {1}", typeof(T).ToString(), source);
        }
    }
}
