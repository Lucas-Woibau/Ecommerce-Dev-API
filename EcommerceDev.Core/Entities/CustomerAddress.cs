namespace EcommerceDev.Core.Entities
{
    public class CustomerAddress : BaseEntity
    {
        protected CustomerAddress() { }
        public CustomerAddress(Guid idCustomer, string recipientName, string addressLine1, string addressLine2, string zipCode, string district, string state, string country, string city)
        {
            IdCustomer = idCustomer;
            RecipientName = recipientName;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            ZipCode = zipCode;
            District = district;
            State = state;
            Country = country;
            City = city;
        }

        public Guid IdCustomer { get; set; }
        public string RecipientName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public string GetFullAddress()
            => $"{AddressLine1} {AddressLine2} {ZipCode} {District} {State} {City}";
    }
}
