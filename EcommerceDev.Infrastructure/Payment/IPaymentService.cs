using System.Net.Http.Json;

namespace EcommerceDev.Infrastructure.Payment;

public interface IPaymentService
{
    Task<string> CreateCustomerAsync(PaymentCustomerModel customer);
    Task<PaymentOrderResponseModel> CreateOrderAsync(PaymentOrderModel paymentOrderModel);
}