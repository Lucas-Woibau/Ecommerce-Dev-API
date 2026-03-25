using EcommerceDev.Application.Common;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Caching;

namespace EcommerceDev.Application.Queries.Products.GetProductDetails
{
    public class GetProductsDetailsQueryHandler : IHandler<GetProductsDetailsQuery, ResultViewModel<ProductDetailsViewModel>>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cacheService;
        private const string CacheKeyPrefix = "product: ";

        public GetProductsDetailsQueryHandler(IProductRepository repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<ResultViewModel<ProductDetailsViewModel>> HandleAsync(GetProductsDetailsQuery request)
        {
            var cacheKey = $"{CacheKeyPrefix}{request.IdProduct}";

            var cachedProduct = await _cacheService.GetAsync<ProductDetailsViewModel>(cacheKey);

            if (cachedProduct != null)
            {
                return ResultViewModel<ProductDetailsViewModel>.Success(cachedProduct);
            }

            var product = await _repository.GetById(request.IdProduct);

            if (product == null)
            {
                return ResultViewModel<ProductDetailsViewModel>.Error("Product not found.");
            }

            var productDetailsViewModel = ProductDetailsViewModel.FromEntity(product);

            await _cacheService.SetAsync(cacheKey, productDetailsViewModel);

            return ResultViewModel<ProductDetailsViewModel>.Success(productDetailsViewModel);            
        }
    }
}
