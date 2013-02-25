using asi.asicentral.web.Models.Store.PageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.Store
{
    public class PageViewModel
    {
        public IList<CompletedOrders> completedOrders { set; get; }
        public IList<PendingOrders> pendingOrders { set; get; }

        public PageViewModel() 
        {
            completedOrders = new List<CompletedOrders>();
            pendingOrders = new List<PendingOrders>();
        }
    }
}