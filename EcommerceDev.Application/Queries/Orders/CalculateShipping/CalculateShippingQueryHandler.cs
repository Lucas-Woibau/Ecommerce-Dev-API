using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Services;
using EcommerceDev.Infrastructure.Geolocation;
using Microsoft.Extensions.Options;

namespace EcommerceDev.Application.Queries.Orders.CalculateShipping
{
    public class CalculateShippingQueryHandler
        : IHandler<CalculateShippingQuery, ResultViewModel<decimal>>
    {
        private readonly IGeolocationService _geolocationService;
        private readonly IOrderDomainService _orderDomainService;
        private readonly GeolocationSettings _geolocationsSettings;

        public CalculateShippingQueryHandler(
            IGeolocationService geolocationService,
            IOrderDomainService orderDomainService,
            IOptions<GeolocationSettings> options)
        {
            _geolocationService = geolocationService;
            _orderDomainService = orderDomainService;
            _geolocationsSettings = options.Value;
        }

        public async Task<ResultViewModel<decimal>> HandleAsync(CalculateShippingQuery request)
        {
            var distanceInKm = await _geolocationService.GetDistance
                (_geolocationsSettings.Origin, request.ZipCode);

            var items = request.Items.Select(i => new OrderItem(i.IdProduct, i.Quantity, 0)).ToList();

            var totalShippingCost = _orderDomainService.CalculateShippingCost(distanceInKm, items);

            if (totalShippingCost == -1)
            {
                return ResultViewModel<decimal>.Error("Erro ao calcular frete.");
            }

            return new ResultViewModel<decimal>(totalShippingCost);
        }
    }
}
