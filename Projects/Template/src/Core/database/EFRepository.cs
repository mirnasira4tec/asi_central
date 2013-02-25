using asi.asicentral.interfaces;
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

        public virtual void Add(object entity)
        {
            if (entity == null) throw new Exception("You cannot add a null entity");
            if (!(entity is T)) throw new Exception("Invalid entity type for this class");
            T entityValue = entity as T;
            _context.Supports(entity.GetType());
            _context.GetSet<T>().Add(entityValue);
        }

        public virtual void Delete(object entity)
        {
            if (entity == null) throw new Exception("You cannot add a null entity");
            _context.Supports(entity.GetType());
            _context.Entry(entity).State = System.Data.EntityState.Deleted;
        }

        public virtual IQueryable<T> GetAll(bool readOnly = false)
        {
            if (readOnly) return _context.GetSet<T>().AsNoTracking() as IQueryable<T>;
            else return _context.GetSet<T>() as IQueryable<T>;
        }

        public virtual IQueryable<T> GetAll(string include, bool readOnly = false)
        {
            _context.Supports(typeof(T));
            if (string.IsNullOrEmpty(include))
            {
                if (readOnly) return _context.GetSet<T>().AsNoTracking() as IQueryable<T>;
                else return _context.GetSet<T>() as IQueryable<T>;
            }
            else
            {
                if (readOnly) return _context.GetSet<T>().AsNoTracking().Include(include) as IQueryable<T>;
                else return _context.GetSet<T>().Include(include) as IQueryable<T>;
            }
        }

        public virtual void Update(T entity)
        {
            _context.Supports(typeof(T));
            _context.Entry(entity).State = System.Data.EntityState.Modified;
            _context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        #endregion IRepository

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
            //no unmanaged resource to free at this point
        }

        #endregion IDisposable
    }
}
