using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    /// <summary>
    /// Used to access persistence logic
    /// </summary>
    public interface IObjectService : IDisposable
    {
        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity"></param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Used to update an object currently not attached
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update<T>(T entity) where T : class;

        /// <summary>
        /// Get a queryable object for the class
        /// </summary>
        /// <param name="include">Include condition for retrieving the data</param>
        /// <returns></returns>
        IQueryable<T> GetAll<T>(bool readOnly = false) where T : class;

        /// <summary>
        /// Get a queryable object for the class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="include">Include condition for retrieving the data</param>
        /// <param name="readOnly">Whether the data will be used to update the original records</param>
        /// <returns></returns>
        IQueryable<T> GetAll<T>(string include, bool readOnly = false) where T : class;

        /// <summary>
        /// Saves any pending changes, returns number of records updated
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
