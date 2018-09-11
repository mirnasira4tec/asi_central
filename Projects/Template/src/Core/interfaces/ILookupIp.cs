using System;
using System.Web;

namespace asi.asicentral.interfaces
{
    interface ILookupIp
    {
        string GetCountry(string ipAddress);
    }
}
