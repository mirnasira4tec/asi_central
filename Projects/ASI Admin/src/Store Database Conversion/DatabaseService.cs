using asi.asicentral.database;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion
{
    public class DatabaseService : IDisposable
    {
        ASIInternetContext _asiInternetContext;
        StoreContext _storeContext;
        ILogService _logService;

        public DatabaseService(DatabaseTarget target)
        {
            _asiInternetContext = new ASIInternetContext("ASIInternetContext" + target);
            _storeContext = new StoreContext("ProductContext" + target);
            _logService = LogService.GetLog(this.GetType());
        }

        public int GetLegacyCount()
        {
            var count = _asiInternetContext.Orders.Count();
            return count;
        }

        public int GetNewCount()
        {
            var count = _storeContext.StoreOrders.Count();
            return count;
        }

        private bool LegacyExists(int id)
        {
            return _storeContext.StoreOrders.Where(order => order.LegacyId == id).Count() > 0;
        }

        public void ProcessLegacyRecords(int from, int to)
        {
            List<LegacyOrder> legacyOrders = _asiInternetContext.Orders.Skip(from).Take(to - from).ToList();
            foreach (LegacyOrder order in legacyOrders)
            {
                if (!LegacyExists(order.Id))
                {
                    StoreOrder newOrder = new StoreOrder();
                    newOrder.ApprovedBy = "Unknwon";
                    newOrder.ApprovedDate = newOrder.UpdateDate;
                    newOrder.Campaign = order.Campaign;
                    newOrder.CompletedStep = order.CompletedStep;
                    newOrder.ContextId = order.ContextId;
                    newOrder.CreateDate = order.DateCreated.HasValue ? order.DateCreated.Value : DateTime.MinValue;
                    newOrder.ExternalReference = order.ExternalReference;
                    newOrder.IPAdd = order.IPAdd;
                    newOrder.IsCompleted = order.Status.HasValue ? order.Status.Value : false;
                    newOrder.LegacyId = order.Id;
                    newOrder.ProcessStatus = order.ProcessStatus;
                    newOrder.UpdateDate = newOrder.CreateDate;
                    newOrder.UpdateSource = "Migration Process - " + DateTime.Now;
                    //@todo billing information (maybe) 
                    //@todo order details
                    _storeContext.StoreOrders.Add(newOrder);
                }
                else
                {
                    _logService.Debug("Order is already present in target database: " + order.Id);
                }
            }
            _storeContext.SaveChanges();
        }

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
                _asiInternetContext.Dispose();
                _storeContext.Dispose();
            }
            //no unmanaged resource to free at this point
        }

        #endregion IDisposable
    }
}
