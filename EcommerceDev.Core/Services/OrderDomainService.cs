using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Core.Services;

public class OrderDomainService : IOrderDomainService
{
    private const decimal PricePerKm = 30;
    private const decimal PricePerUnit = 2.5m;

    private readonly IProductRepository _productRepository;

    public OrderDomainService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public decimal CalculateShippingCost(int distanceKm, List<OrderItem> items)
    {
        var totalPriceKm = PricePerKm * distanceKm;

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
            var product = await _productRepository.GetById(item.IdProduct);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            item.Price = product.Price;
        }
    }
}