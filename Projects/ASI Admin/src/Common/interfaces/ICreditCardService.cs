using asi.asicentral.model;
using asi.asicentral.model.store;
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
        /// Validates a credit card
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        bool Validate(CreditCard creditCard);

        /// <summary>
        /// Store a credit card in the volt
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        string Store(CreditCard creditCard);
    }
}
