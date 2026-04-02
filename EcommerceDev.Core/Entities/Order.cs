using EcommerceDev.Core.Enums;

namespace EcommerceDev.Core.Entities
{
    public class Order : BaseEntity
    {
        protected Order() { }
        public Order(Guid idCustomer, Guid deliveryAddressId, decimal shippingPrice, List<OrderItem> items)
        {
            IdCustomer = idCustomer;
            Status = OrderStatus.Created;
            DeliveryAddressId = deliveryAddressId;
            ShippingPrice = shippingPrice;
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
        public string? IdExternalOrder { get; set; }
        public string? PaymentUrl { get; set; }
        public List<OrderItem> Items { get; private set; }
        public List<OrderUpdate> Updates { get; private set; }

        public void MarkAsPaymentPending()
        {
            if (Status != OrderStatus.Created)
            {
                Console.WriteLine("[Order] Order is in invalid state for payment.");

                throw new Exception("rder is in invalid state for payment.");
            }

            Status = OrderStatus.PaymentPending;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPaymentExpired()
        {
            if (Status != OrderStatus.PaymentPending)
            {
                Console.WriteLine("[Order] Order is in invalid state for payment expiration.");

                throw new Exception("rder is in invalid state for payment expiration.");
            }

            Status = OrderStatus.PaymentExpired;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetTotalProductsPrice(decimal totalProductsPrice)
        {
            TotalProductsPrice = totalProductsPrice;
        }
    }
}
