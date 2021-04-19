using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreCreditCard
    {
        public int Id { get; set; }
        public string CardHolderName { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string ExternalReference { get; set; }

        public string TokenId { get; set; }
        public string AuthReference { get; set; }
        // new properties for JetPay
        public string CompanyName { get; set; }
        public string RequestToken { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string AVS_Result { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public override string ToString()
        {
            return Id + " - " + CardHolderName;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreAddress card = obj as StoreAddress;
            if (card != null) equals = card.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
