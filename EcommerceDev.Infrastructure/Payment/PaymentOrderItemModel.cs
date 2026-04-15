namespace EcommerceDev.Infrastructure.Payment;

public class PaymentOrderItemModel
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}