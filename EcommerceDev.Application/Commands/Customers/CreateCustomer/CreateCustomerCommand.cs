namespace EcommerceDev.Application.Commands.Customers.CreateCustomer
{
    public class CreateCustomerCommand
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Document { get; set; } = string.Empty;
    }
}
