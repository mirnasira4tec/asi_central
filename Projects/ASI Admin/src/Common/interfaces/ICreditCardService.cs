using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    public interface ICreditCardService
    {
        /// <summary>
        /// Store a credit card in the volt
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        string Store(CreditCard creditCard);

        /// <summary>
        /// Permanently removes a credit card
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);
    }
}
