using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services.interfaces
{
    /// <summary>
    /// The main "container" object that implements the Service Locator pattern
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Creates or finds the default instance of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetInstance<T>();
    }
}
