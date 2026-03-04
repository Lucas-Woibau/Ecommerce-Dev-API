using EcommerceDev.Application.Commands.Categories.CreateCategory;
using EcommerceDev.Application.Commands.Orders.CreateOrder;
using EcommerceDev.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand request)
        {
            var result = await
                _mediator.DispatchAsync<CreateOrderCommand, ResultViewModel<Guid>>(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
