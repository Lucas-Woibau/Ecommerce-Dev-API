using EcommerceDev.Core.Entities;

namespace EcommerceDev.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<Guid> Create(Customer customer);
        Task<Guid> CreateAddress(CustomerAddress customerAddress);
        Task<CustomerAddress?> GetAddress(Guid id);
    }
}
