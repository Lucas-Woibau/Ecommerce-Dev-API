using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;

namespace EcommerceDev.Core.Services;

public class OrderDomainService : IOrderDomainService
{
    private const decimal PricePerKm = 30;
    private const decimal PricePerUnit = 2.5m;
    private const int MaximumAllowedDistanceKm = 250;

    private readonly IProductRepository _repository;

    public OrderDomainService(IProductRepository repository)
    {
        _repository = repository;
    }

    public decimal CalculateShippingCost(int distanceInKm, List<OrderItem> items)
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("No items found.");
        }

        if (distanceInKm > MaximumAllowedDistanceKm || distanceInKm < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(distanceInKm), distanceInKm, "Distance out of range.");
        }

        var totalPriceKm = distanceInKm == 0 ?
            PricePerKm :
            PricePerKm * distanceInKm;

        var totalUnits = items.Sum(i => i.Quantity);
        var totalPriceUnits = PricePerUnit * totalUnits;

        var total = totalPriceUnits + totalPriceKm;

        return total;
    }

    public async Task<decimal> CalculateProductOrderTotal(List<OrderItem> items)
    {
        decimal total = 0;

        foreach (var item in items)
        {
            total += item.Price * item.Quantity;
        }

        return total;
    }

    public async Task UpdateProductPrices(Order order)
    {
        foreach (var item in order.Items)
        {
            var product = await _repository.GetById(item.IdProduct);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            item.Price = product.Price;
        }
    }
}