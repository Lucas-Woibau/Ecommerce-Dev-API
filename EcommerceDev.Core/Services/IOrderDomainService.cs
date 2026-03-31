using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Services;

namespace EcommerceDev.Core.Services
{
    public interface IOrderDomainService
    {
        decimal CalculateShippingCost(int distanceKm, List<OrderItem> items);
    }
}

public class OrderDomainService : IOrderDomainService
{
    private const decimal PricePerKm = 30;
    private const decimal PricePerUnit = 2.5m;

    public decimal CalculateShippingCost(int distanceKm, List<OrderItem> items)
    {
        var totalPriceKm = PricePerKm * distanceKm;

        var totalUnits = items.Sum(i => i.Quantity);
        var totalPriceUnits = PricePerUnit * totalUnits;

        var total = totalPriceUnits + totalPriceKm;

        return total;
    }
}