namespace EcommerceDev.Core.Enums
{
    public enum OrderStatus
    {
        Created = 1,
        PaymentPending = 2,
        Confirmed = 3,
        Picking = 4,
        Shipped = 5,
        Delivered = 6,
        Cancelled = 7,
        PaymentExpired = 8
    }
}
