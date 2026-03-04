namespace EcommerceDev.Core.Entities
{
    public class OrderUpdate : BaseEntity
    {
        protected OrderUpdate() { }
        public OrderUpdate(string description, Guid idOrder)
        {
            Description = description;
            IdOrder = idOrder;
        }

        public string Description { get; set; }
        public Guid IdOrder { get; set; }
    }
}
