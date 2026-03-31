using EcommerceDev.Core.Enums;

namespace EcommerceDev.Core.Entities
{
    public class Order : BaseEntity
    {
        protected Order() { }
        public Order(Guid idCustomer, Guid deliveryAddressId, decimal shippingPrice, decimal totalProductsPrice, List<OrderItem> items)
        {
            IdCustomer = idCustomer;
            Status = OrderStatus.Created;
            DeliveryAddressId = deliveryAddressId;
            ShippingPrice = shippingPrice;
            TotalProductsPrice = totalProductsPrice;
            Items = items;
            Updates = [];
        }

        public Guid IdCustomer { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime? ConfirmationDate { get; private set; }
        public DateTime? ShippingDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public Guid DeliveryAddressId { get; private set; }
        public CustomerAddress DeliveryAddress { get; private set; }
        public decimal ShippingPrice { get; private set; }
        public decimal TotalProductsPrice { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public List<OrderUpdate> Updates { get; private set; }

        public void MarkAsConfirmed()
        {
            if (Status != OrderStatus.Created)
            {
                Console.WriteLine("[Order] Order is in invalid state for confirmation");

                throw new Exception("rder is in invalid state for confirmation");
            }

            Status = OrderStatus.Confirmed;
        }
    }
}
