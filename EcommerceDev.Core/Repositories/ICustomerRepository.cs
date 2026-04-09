using EcommerceDev.Core.Entities;

namespace EcommerceDev.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<Guid> Create(Customer customer);
        Task<Guid> CreateAddress(CustomerAddress address);
        Task<CustomerAddress?> GetAddress(Guid id);
        Task<Customer?> GetById(Guid id);
        Task<Customer?> GetByEmail(string email);
        Task<bool> EmailExists(string email);
        Task<List<CustomerAddress>> GetAllAddresses(Guid customerId);
        Task Update(Customer customer);
    }
}
