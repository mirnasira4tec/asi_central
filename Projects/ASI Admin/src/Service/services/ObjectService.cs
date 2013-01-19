using asi.asicentral.services.interfaces;
using asi.asicentral.database.mappings;
using StructureMap;
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
        private static Container _container = new Container(new EFRegistry());
        private IDictionary<string, IUnitOfWork> repositories = new Dictionary<string, IUnitOfWork>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ObjectService()
        {
        }

        #region IObjectService

        public void Add<T>(T entity) where T : class
        {
            if (entity == null) throw new Exception("You cannot add a null object");
            IRepository<T> repository = GetRepository<T>();
            repository.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            if (entity == null) throw new Exception("You cannot delete a null object");
            IRepository<T> repository = GetRepository<T>();
            repository.Delete(entity);
        }

        public T Update<T>(T entity) where T : class
        {
            if (entity == null) throw new Exception("You cannot Update a null object");
            IRepository<T> repository = GetRepository<T>();
            repository.Update(entity);
            return entity;
        }

        public IQueryable<T> GetAll<T>(bool readOnly = false) where T : class
        {
            IRepository<T> repository = GetRepository<T>();
            IQueryable<T> query = repository.GetAll(readOnly);
            return query;
        }

        public int SaveChanges()
        {
            int i = 0;
            foreach (IUnitOfWork repository in repositories.Values) i = i + repository.SaveChanges();
            return i;
        }

        #endregion IObjectService

        #region IDisposable

        public void Dispose()
        {
            foreach (IUnitOfWork repository in repositories.Values)
            {
                IDisposable dispose = repository as IDisposable;
                if (dispose != null) dispose.Dispose();
            }
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
            if (repositories.ContainsKey(name))
            {
                repository = repositories[name] as IRepository<T>;
            }
            else
            {
                lock (repositories)
                {
                    repository = _container.GetInstance<IRepository<T>>();
                    repositories.Add(name, repository as IUnitOfWork);
                }
            }
            return repository;
        }
    }
}
