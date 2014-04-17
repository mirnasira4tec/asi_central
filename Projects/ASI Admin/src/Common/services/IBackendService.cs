using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using System;
using System.Collections.Generic;

namespace asi.asicentral.services
{

    interface IBackendService
    {

        bool PlaceOrder(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes);
    }
}
