using EcommerceDev.Application.Commands.Orders.CreateOrder;
using EcommerceDev.Application.Common;
using EcommerceDev.Application.Queries.Orders.CalculateShipping;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers;

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
        var result = await _mediator
            .DispatchAsync<CreateOrderCommand, ResultViewModel<Guid>>(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpPost("calculate-shipping")]
    public async Task<IActionResult> CalculateShipping(CalculateShippingQuery request)
    {
        var result = await _mediator
            .DispatchAsync<CalculateShippingQuery, ResultViewModel<decimal>>(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}