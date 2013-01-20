using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services.interfaces
{
    /// <summary>
    /// Created to accommodate with the need for the application to be able to talk to multiple databases
    /// We will need to make sure each repository is associated with the appropriate context
    /// </summary>
    public interface IValidatedContext : IDisposable, IUnitOfWork
    {
        /// <summary>
        /// Throws an exception if the context does not support certain classes
        /// </summary>
        /// <param name="type"></param>
        void Supports(Type type);

        /// <summary>
        /// Gives access to the DbSet for the given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DbSet GetSet(Type type);

        /// <summary>
        /// To be used to attach a simple model and update the database
        /// Entry(entity).State = EntityState.Modified;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
