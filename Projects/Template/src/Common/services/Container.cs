using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    /// <summary>
    /// Wrapper around the StructureMap Container in case we change the wiring plumbing later
    /// </summary>
    public class Container : IContainer
    {
        private StructureMap.Container _container;

        /// <summary>
        /// Constructor, passing Structure Map registry to be used for the implementation
        /// </summary>
        /// <param name="registry"></param>
        public Container(Registry registry)
        {
            _container = new StructureMap.Container(registry);
        }

        /// <summary>
        /// Gets instance of a Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
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
                if (_container != null)
                {
                    _container.Dispose();
                    _container = null;
                }
            }
            //no unmanaged resource to free at this point
        }

        /// <summary>
        /// Destructor for the class
        /// </summary>
        ~Container()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}
