using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    /// <summary>
    /// Interface to the fulfilment system which will be the one to actually process the orders
    /// </summary>
    public interface IFulfilmentService : IDisposable
    {
        /// <summary>
        /// Take an order from the store and pass it to the Fulfilment service
        /// </summary>
        /// <param name="order"></param>
        /// <param name="application"></param>
        void Process(Order order, OrderDetailApplication application);
    }
}
