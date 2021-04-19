namespace asi.asicentral.model
{
    public class CreditCard
    {
        public string Type { get; set; }
        public string Number { get; set; }
        public string MaskedPAN { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string CardHolderName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string ExternalReference { get; set; }
        public string TokenId { get; set; }
        public string AuthReference { get; set; }

        // new properties for JetPay
        public string CompanyName { get; set; }
        public string RequestToken { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string AVS_Result { get; set; }
    }
}
