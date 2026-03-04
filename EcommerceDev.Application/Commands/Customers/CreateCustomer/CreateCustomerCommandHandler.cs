using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;

namespace EcommerceDev.Application.Commands.Customers.CreateCustomer
{
    public class CreateCustomerCommandHandler : IHandler<CreateCustomerCommand, ResultViewModel<Guid>>
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel<Guid>> HandleAsync(CreateCustomerCommand request)
        {
            var customer = new Customer(request.FullName, request.Email, request.PhoneNumer,
                request.BirthDate, request.Document);

            await _repository.Create(customer);

            return ResultViewModel<Guid>.Success(customer.Id);
        }
    }
}
