using EcommerceDev.Application.Common;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Caching;

namespace EcommerceDev.Application.Queries.Products.GetAllProducts
{
    public class GetAllProductsQueryHandler : 
        IHandler<GetAllProductsQuery, ResultViewModel<List<GetAllProductsItemViewModel>>>
    {
        private readonly ICacheService _cacheService;
        private readonly IProductRepository _productRepository;
        private const string CacheKey = "products:all";

        public GetAllProductsQueryHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            _cacheService = cacheService;
            _productRepository = productRepository;
        }

        public async Task<ResultViewModel<List<GetAllProductsItemViewModel>>> HandleAsync(GetAllProductsQuery request)
        {
            var cachedProducts = await _cacheService.GetAsync<List<GetAllProductsItemViewModel>>(CacheKey);

            if (cachedProducts != null)
            {
                return ResultViewModel<List<GetAllProductsItemViewModel>>.Success(cachedProducts);
            }

            var products = await _productRepository.GetAll();

            var productsViewModel = products.Select(p => new GetAllProductsItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
            }).ToList();

            await _cacheService.SetAsync(CacheKey, productsViewModel);

            return ResultViewModel<List<GetAllProductsItemViewModel>>.Success(productsViewModel);
        }
    }
}
