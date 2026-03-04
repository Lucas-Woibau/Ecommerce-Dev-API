namespace EcommerceDev.Application.Commands.Customers.CreateCustomerAddress
{
    public class CreateCustomerAddressCommand
    {
        public Guid IdCustomer { get; set; } 
        public string RecipientName { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
