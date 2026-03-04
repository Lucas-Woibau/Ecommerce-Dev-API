namespace EcommerceDev.Core.Entities
{
    public class OrderItemReview : BaseEntity
    {
        protected OrderItemReview() { }
        public OrderItemReview(Guid idOrderItem, Guid idCustomer, string title, string description, int score)
        {
            IdOrderItem = idOrderItem;
            IdCustomer = idCustomer;
            Title = title;
            Description = description;
            Score = score;
        }

        public Guid IdOrderItem { get; set; }
        public OrderItem OrderItem { get; set; }
        public Guid IdCustomer { get; set; }
        public Customer Customer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
    }
}
