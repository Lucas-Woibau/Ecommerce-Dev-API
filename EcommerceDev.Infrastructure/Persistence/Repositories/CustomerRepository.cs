using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceDev.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly EcommerceDbContext _context;
    public CustomerRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Create(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        
        return customer.Id;
    }

    public async Task<Guid> CreateAddress(CustomerAddress address)
    {
        await _context.CustomerAddresses.AddAsync(address);
        await _context.SaveChangesAsync();
        
        return address.Id;
    }

    public async Task<CustomerAddress?> GetAddress(Guid id)
    {
        return await _context.CustomerAddresses.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Customer?> GetById(Guid id)
    {
        return await _context.Customers.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Customer?> GetByEmail(string email)
    {
        return await _context.Customers.SingleOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email);
    }

    public async Task<List<CustomerAddress>> GetAllAddresses(Guid customerId)
    {
        return await _context.CustomerAddresses
            .Where(x => x.IdCustomer == customerId)
            .ToListAsync();
    }

    public async Task Update(Customer customer)
    {
        _context.Customers.Update(customer);

        await _context.SaveChangesAsync();
    }
}