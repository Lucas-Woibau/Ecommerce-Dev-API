using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;

namespace EcommerceDev.Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IHandler<CreateOrderCommand, ResultViewModel<Guid>>
    {
        private readonly IOrderRepository _repository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _repository = orderRepository;
        }

        public async Task<ResultViewModel<Guid>> HandleAsync(CreateOrderCommand request)
        {
            var order = new Order(
                request.IdCustomer,
                request.DeliveryAddressId,
                100,
                1000,
                request.Items.Select(i => 
                new OrderItem(i.IdProduct, i.Quantity, 5)).ToList());

            await _repository.Create(order);

            return ResultViewModel<Guid>.Success(order.Id);
        }
    }
}
