using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommerceDev.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDbContext _context;

        public ProductRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<Product?> GetById(Guid id)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
