using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.model.store;

namespace asi.asicentral.web.Models.Store.Application
{
    public class ApplicationPageModel
    {
        public Order order { set; get; }
        public OrderDetailApplication application { set; get; }
    }
}