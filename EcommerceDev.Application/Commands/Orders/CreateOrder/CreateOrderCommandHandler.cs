using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Events;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Core.Services;
using EcommerceDev.Infrastructure.Geolocation;
using EcommerceDev.Infrastructure.Messaging;
using Microsoft.Extensions.Options;

namespace EcommerceDev.Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IHandler<CreateOrderCommand, ResultViewModel<Guid>>
    {
        private readonly IOrderRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IGeolocationService _geolocationService;
        private readonly IOrderDomainService _orderDomainService;
        private readonly ICustomerRepository _customerRepository;
        private readonly GeolocationSettings _geolocationSettings;

        public CreateOrderCommandHandler(
            IOrderRepository repository,
            IEventPublisher eventPublisher,
            IGeolocationService geolocationService,
            IOrderDomainService orderDomainService,
            ICustomerRepository customerRepository,
            IOptions<GeolocationSettings> geolocationSettingsOptions)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _geolocationService = geolocationService;
            _orderDomainService = orderDomainService;
            _customerRepository = customerRepository;
            _geolocationSettings = geolocationSettingsOptions.Value;
        }

        public async Task<ResultViewModel<Guid>> HandleAsync(CreateOrderCommand request)
        {
            var address = await _customerRepository.GetAddress(request.DeliveryAddressId);

            if (address == null)
            {
                return ResultViewModel<Guid>.Error("Endereço não encontrado.");
            }

            var totalShippingCost = await CalculateShipping(request, address.GetFullAddress());

            if (totalShippingCost == -1)
            {
                return ResultViewModel<Guid>.Error("Erro ao calcular o frete.");
            }

            var order = new Order(
                request.IdCustomer,
                request.DeliveryAddressId,
                totalShippingCost,
                1000,
                request.Items.Select(i =>
                new OrderItem(i.IdProduct, i.Quantity, 5)).ToList());

            await _repository.CreateAsync(order);

            var @event = new OrderCreatedEvent(order.Id);

            await _eventPublisher.PublishAsync(@event);

            return ResultViewModel<Guid>.Success(order.Id);
        }

        private async Task<decimal> CalculateShipping(CreateOrderCommand request, string destination)
        {
            var distanceInKm = await _geolocationService.GetDistance
                (_geolocationSettings.Origin, destination);

            var items = request.Items.Select(i => new OrderItem(i.IdProduct, i.Quantity, 0)).ToList();

            var totalShippingCost = _orderDomainService.CalculateShippingCost(distanceInKm, items);

            return totalShippingCost;
        }
    }
}
