using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services.interfaces
{
    /// <summary>
    /// Used to access business logic
    /// </summary>
    public interface IObjectService : IDisposable
    {
        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity"></param>
        void Add<T>(T entity);

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <param name="entity"></param>
        void Delete<T>(T entity);

        /// <summary>
        /// Get a queryable object for the class
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll<T>(bool readOnly = false);

        /// <summary>
        /// Saves any pending changes, returns number of records updated
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
