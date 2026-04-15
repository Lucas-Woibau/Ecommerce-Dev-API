using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceDev.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly EcommerceDbContext _context;

    public OrderRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        
        return order.Id;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return  await _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .SingleOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}