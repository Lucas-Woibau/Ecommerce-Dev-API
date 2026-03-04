using EcommerceDev.Core.Entities;

namespace EcommerceDev.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Guid> Create(Product product);
        Task<List<Product>> GetAll();
        Task<Product?> GetById(Guid id);
    }
}
