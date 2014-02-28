using asi.asicentral.interfaces;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util.store.magazinesadvertising
{
    public static class Update
    {

        public static void UpdateDateUTCAndSource<T>(this T o, [CallerFilePath] string fileName = "", [CallerMemberName] string methodName = "") where T : IDateUTCAndSource
        {
            if (o.CreateDate == null || o.CreateDate == default(DateTime)) o.CreateDate = DateTime.UtcNow;
            o.UpdateDate = DateTime.UtcNow;
            fileName = fileName.Substring(fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            o.UpdateSource = string.Format("{0} - {1}", fileName, methodName);
        }
    }
}
