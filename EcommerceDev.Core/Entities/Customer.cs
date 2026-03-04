namespace EcommerceDev.Core.Entities
{
    public class Customer : BaseEntity
    {
        protected Customer() { }
        public Customer(string fullName, string email, string phoneNumer, DateTime birthDate, string document)
        {
            FullName = fullName;
            Email = email;
            PhoneNumer = phoneNumer;
            BirthDate = birthDate;
            Document = document;

            Addresses = [];
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumer { get; set; }
        public DateTime BirthDate { get; set; }
        public string Document { get; set; }
        public List<CustomerAddress> Addresses { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderItemReview> Reviews { get; set; }
    }
}
