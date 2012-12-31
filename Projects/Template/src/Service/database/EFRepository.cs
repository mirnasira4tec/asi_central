using asi.asicentral.model;
using asi.asicentral.services.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database
{
    public class EFRepository<T> : IRepository<T>, IDisposable, IUnitOfWork
    {
        IValidatedContext _context = null;

        public EFRepository(IValidatedContext context)
        {
            _context = context;
            ReadOnly = false;
        }

        public bool ReadOnly { get; set; }

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
            _context.GetSet(typeof(T)).Remove(entity);
        }

        public IQueryable<T> GetAll(bool readOnly = false)
        {
            _context.Supports(typeof(T));
            if (readOnly) return _context.GetSet(typeof(T)).AsNoTracking() as IQueryable<T>;
            else return _context.GetSet(typeof(T)) as IQueryable<T>;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
