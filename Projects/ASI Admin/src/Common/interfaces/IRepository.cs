﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    /// <summary>
    /// Repository pattern which will allow retrieving model from persistence
    /// </summary>
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(object entity);

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <param name="entity"></param>
        void Delete(object entity);

        /// <summary>
        /// Used to update an object currently not attached
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Get a queryable object for the class
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll(bool readOnly = false);

        /// <summary>
        /// Saves any pending changes, returns number of records updated
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
