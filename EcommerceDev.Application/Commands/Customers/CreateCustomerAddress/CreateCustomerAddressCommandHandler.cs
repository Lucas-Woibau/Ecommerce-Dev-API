using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;

namespace EcommerceDev.Application.Commands.Customers.CreateCustomerAddress
{
    public class CreateCustomerAddressCommandHandler : IHandler<CreateCustomerAddressCommand, ResultViewModel<Guid>>
    {
        private readonly ICustomerRepository _repository;
        public CreateCustomerAddressCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<Guid>> HandleAsync(CreateCustomerAddressCommand request)
        {
            var address = new CustomerAddress(
                request.IdCustomer,
                request.RecipientName,
                request.AddressLine1,
                request.AddressLine2,
                request.ZipCode,
                request.District,
                request.State,
                request.Country,
                request.City
            );

            await _repository.CreateAddress(address);

            return ResultViewModel<Guid>.Success(address.Id);
        }
    }
}
