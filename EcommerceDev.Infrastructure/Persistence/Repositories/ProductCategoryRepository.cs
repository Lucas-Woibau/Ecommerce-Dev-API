using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Persistence;

namespace EcommerceDev.Infrastructure.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly EcommerceDbContext _context;
    
    public ProductCategoryRepository(EcommerceDbContext context)
    {
          _context = context;
    }
    
    public async Task<Guid> Create(ProductCategory productCategory)
    {
        await _context.ProductCategories.AddAsync(productCategory);
        await _context.SaveChangesAsync();
        
        return productCategory.Id;
    }
}