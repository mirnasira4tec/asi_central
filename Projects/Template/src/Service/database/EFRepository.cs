using asi.asicentral.services.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database
{
    public class EFRepository<T> : IRepository<T>, IDisposable, IUnitOfWork where T : class
    {
        IValidatedContext _context = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="context">The context to use for the repository</param>
        public EFRepository(IValidatedContext context)
        {
            _context = context;
        }

        #region IRepository

        public void Add(object entity)
        {
            if (entity == null) throw new Exception("You cannot add a null entity");
            if (!(entity is T)) throw new Exception("Invalid entity type for this class");
            _context.Supports(entity.GetType());
            _context.GetSet(typeof(T)).Add(entity);
        }

        public void Delete(object entity)
        {
            if (entity == null) throw new Exception("You cannot add a null entity");
            _context.Supports(entity.GetType());
            _context.Entry(entity).State = System.Data.EntityState.Deleted;
        }

        public IQueryable<T> GetAll(bool readOnly = false)
        {
            _context.Supports(typeof(T));
            if (readOnly) return _context.GetSet(typeof(T)).AsNoTracking() as IQueryable<T>;
            else return _context.GetSet(typeof(T)) as IQueryable<T>;
        }

        public void Update(T entity)
        {
            _context.Supports(typeof(T));
            _context.Entry(entity).State = System.Data.EntityState.Modified;
            _context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        #endregion IRepository

        #region IDisposable

        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion IDisposable
    }
}
