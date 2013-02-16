using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    /// <summary>
    /// Represents a set of operations which need to be performed at once
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves any pending changes, returns number of records updated
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
