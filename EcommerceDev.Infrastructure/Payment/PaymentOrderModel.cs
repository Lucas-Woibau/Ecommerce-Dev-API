namespace EcommerceDev.Infrastructure.Payment;

public class PaymentOrderModel
{
    public string IdExternalCustomer { get; set; }
    public List<PaymentOrderItemModel> Items { get; set; }
}