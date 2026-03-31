using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommerceDev.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDbContext _context;

        public OrderRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public Task<Guid> Create(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        public Task CreateImage(ProductImage productImage)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .SingleOrDefaultAsync(o => o.Id == id);
        }

        public Task<ProductImage?> GetImageById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
