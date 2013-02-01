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

        public Container(Registry registry)
        {
            _container = new StructureMap.Container(registry);
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
