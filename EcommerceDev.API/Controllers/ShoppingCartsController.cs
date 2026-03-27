using EcommerceDev.Application.Commands.ShoppingCarts.CreateOrUpdateShoppingCart;
using EcommerceDev.Application.Common;
using EcommerceDev.Application.Common.ShoppingCart;
using EcommerceDev.Application.Queries.ShoppingCarts.GetShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers
{
    [ApiController]
    [Route("api/shopping-carts")]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShoppingCartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetShopppingCartQuery(id);

            var result = await _mediator
                .DispatchAsync<GetShopppingCartQuery, ResultViewModel<List<ProductItemShoppingCartModel>>>(query);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, CreateOrUpdateShoppingCartCommand command)
        {
            command.IdCustomer = id;

            var result = await _mediator
                .DispatchAsync<CreateOrUpdateShoppingCartCommand, ResultViewModel<bool>>(command);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
