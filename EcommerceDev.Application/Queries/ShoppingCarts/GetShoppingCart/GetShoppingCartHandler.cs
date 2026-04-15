using EcommerceDev.Application.Common;
using EcommerceDev.Application.Common.ShoppingCart;
using EcommerceDev.Infrastructure.Caching;

namespace EcommerceDev.Application.Queries.ShoppingCarts.GetShoppingCart;

public class GetShoppingCartHandler
    : IHandler<GetShoppingCartQuery, ResultViewModel<List<ProductItemShoppingCartModel>>>
{
    private readonly ICacheService _cacheService;

    public GetShoppingCartHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<List<ProductItemShoppingCartModel>>> HandleAsync(GetShoppingCartQuery request)
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