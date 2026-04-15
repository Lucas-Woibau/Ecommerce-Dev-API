using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcommerceDev.Infrastructure.Repositories;

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
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Reviews)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateImage(ProductImage productImage)
    {
        await _context.ProductImages.AddAsync(productImage);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductImage?> GetImageById(Guid id)
    {
        var  image = await _context.ProductImages.SingleOrDefaultAsync(p => p.Id == id);
        
        return image;
    }
}