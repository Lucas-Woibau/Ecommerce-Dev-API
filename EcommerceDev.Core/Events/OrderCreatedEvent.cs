namespace EcommerceDev.Core.Events
{
    public class OrderCreatedEvent
    {
        public OrderCreatedEvent(Guid idOrder)
        {
            IdOrder = idOrder;
        }

        public Guid IdOrder { get; private set; }
    }
}
