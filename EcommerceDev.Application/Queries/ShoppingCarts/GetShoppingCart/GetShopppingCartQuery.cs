using EcommerceDev.Application.Common;
using EcommerceDev.Application.Common.ShoppingCart;
using EcommerceDev.Infrastructure.Caching;

namespace EcommerceDev.Application.Queries.ShoppingCarts.GetShoppingCart
{
    public class GetShopppingCartQuery
    {
        public GetShopppingCartQuery(Guid idCustomer)
        {
            IdCustomer = idCustomer;
        }

        public Guid IdCustomer { get; set; }
    }

    public class GetShoppingCartHandler : IHandler<GetShopppingCartQuery, ResultViewModel<List<ProductItemShoppingCartModel>>>
    {
        private readonly ICacheService _cacheService;

        public GetShoppingCartHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<ResultViewModel<List<ProductItemShoppingCartModel>>> HandleAsync(GetShopppingCartQuery request)
        {
            var cacheKey = request.IdCustomer.ToString();

            var cacheResult = await _cacheService.GetAsync<List<ProductItemShoppingCartModel>>(cacheKey);

            if (cacheResult is null)
            {
                return ResultViewModel<List<ProductItemShoppingCartModel>>.Error("Registro não encontrado.");
            }

            return ResultViewModel<List<ProductItemShoppingCartModel>>.Success(cacheResult);
        }
    }
}
