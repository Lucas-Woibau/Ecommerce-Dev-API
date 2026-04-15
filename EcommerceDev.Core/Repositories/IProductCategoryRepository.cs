namespace EcommerceDev.Core.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<Guid> Create(ProductCategory productCategory);
    }
}
