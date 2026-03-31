using EcommerceDev.Core.Entities;

namespace EcommerceDev.Core.Services
{
    public interface IOrderDomainService
    {
        decimal CalculateShippingCost(int distanceKm, List<OrderItem> items);
        Task<decimal> CalculateProductOrderTotal(List<OrderItem> items);
        Task UpdateProductPrices(Order order);
    }
}
