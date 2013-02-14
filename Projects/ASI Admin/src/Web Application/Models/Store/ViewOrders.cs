using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.Store
{
    public class ViewOrders
    {
        public IList<ClosedOrder> closedOrders { set; get; }
        public IList<OpenOrder> openedOrders { set; get; }

        public ViewOrders() 
        {
            closedOrders = new List<ClosedOrder>();
            openedOrders = new List<OpenOrder>();
        }
    }
}