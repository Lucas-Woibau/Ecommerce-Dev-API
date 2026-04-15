using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EcommerceDev.Infrastructure.Payment;

public class StripePaymentService : IPaymentService
{
    private const string StripeApiKey = "";
    private const string StripeApiBaseUrl = "https://api.stripe.com/v1";
    private const string SuccessUrl = "https://example.com/success";
    private const string Currency = "brl";
    private const string DisplayName = "Pedido EcommerceDev";

    private readonly HttpClient _httpClient;

    public StripePaymentService()
    {
        _httpClient = new HttpClient();
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{StripeApiKey}:"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    }

    public async Task<string> CreateCustomerAsync(PaymentCustomerModel customer)
    {
        var formData = new Dictionary<string, string>
        {
            { "name", customer.FullName },
            { "email", customer.Email }
        };

        if (!string.IsNullOrEmpty(customer.PhoneNumber))
        {
            formData.Add("phone", customer.PhoneNumber);
        }

        var content = new FormUrlEncodedContent(formData);
        var response = await _httpClient.PostAsync($"{StripeApiBaseUrl}/customers", content);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<PaymentCustomerResponseModel>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return responseModel?.Id ?? throw new Exception("Failed to create customer");
    }

    public async Task<PaymentOrderResponseModel> CreateOrderAsync(PaymentOrderModel paymentOrderModel)
    {
        var formData = new Dictionary<string, string>
        {
            { "success_url", SuccessUrl },
            { "mode", "payment" },
            { "customer", paymentOrderModel.IdExternalCustomer },
            { "branding_settings[display_name]", DisplayName }
        };

        // Build line items
        for (int i = 0; i < paymentOrderModel.Items.Count; i++)
        {
            var item = paymentOrderModel.Items[i];
            var unitAmountDecimal = (long)(item.Price * 100); // Convert to cents

            formData.Add($"line_items[{i}][quantity]", item.Quantity.ToString());
            formData.Add($"line_items[{i}][price_data][currency]", Currency);
            formData.Add($"line_items[{i}][price_data][product_data][name]", item.Name);
            formData.Add($"line_items[{i}][price_data][unit_amount_decimal]", unitAmountDecimal.ToString());
        }

        var content = new FormUrlEncodedContent(formData);
        var response = await _httpClient.PostAsync($"{StripeApiBaseUrl}/checkout/sessions", content);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<PaymentOrderResponseModel>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return responseModel ?? throw new Exception("Failed to create checkout session");
    }
}