using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class ObjectService : IObjectService
    {
        //load container to resolve Repository based on the model. No need to load everytime. 
        //Definition is on code so it can be static
        private IContainer _container;
        private IDictionary<string, IUnitOfWork> _repositories = new Dictionary<string, IUnitOfWork>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ObjectService(IContainer container)
        {
            _container = container;
        }

        #region IObjectService

        public virtual void Add<T>(T entity) where T : class
        {
            if (entity == null) throw new Exception("You cannot add a null object");
            IRepository<T> repository = GetRepository<T>();
            repository.Add(entity);
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            if (entity == null) throw new Exception("You cannot delete a null object");
            IRepository<T> repository = GetRepository<T>();
            repository.Delete(entity);
        }

        public virtual T Update<T>(T entity) where T : class
        {
            if (entity == null) throw new Exception("You cannot Update a null object");
            IRepository<T> repository = GetRepository<T>();
            repository.Update(entity);
            return entity;
        }

        public virtual IQueryable<T> GetAll<T>(bool readOnly = false) where T : class
        {
            IRepository<T> repository = GetRepository<T>();
            IQueryable<T> query = repository.GetAll(readOnly);
            return query;
        }

        public virtual int SaveChanges()
        {
            int i = 0;
            foreach (IUnitOfWork repository in _repositories.Values) i = i + repository.SaveChanges();
            return i;
        }

        #endregion IObjectService

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
                if (_repositories != null)
                {
                    foreach (IUnitOfWork repository in _repositories.Values)
                    {
                        IDisposable dispose = repository as IDisposable;
                        if (dispose != null) dispose.Dispose();
                    }
                    _repositories.Clear();
                    _repositories = null;
                }
                if (_container != null)
                {
                    _container.Dispose();
                    _container = null;
                }
            }
            //no unmanaged resource to free at this point
        }

        #endregion IDisposable

        /// <summary>
        /// Method used to lookup and maintain the cache for the repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IRepository<T> GetRepository<T>() where T : class
        {
            IRepository<T> repository = null;
            string name = typeof(T).FullName;
            if (_repositories.ContainsKey(name))
            {
                repository = _repositories[name] as IRepository<T>;
            }
            else
            {
                lock (_repositories)
                {
                    repository = _container.GetInstance<IRepository<T>>();
                    _repositories.Add(name, repository as IUnitOfWork);
                }
            }
            return repository;
        }
    }
}
