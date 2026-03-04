using EcommerceDev.Application.Commands.Customers.CreateCustomer;
using EcommerceDev.Application.Commands.Products.CreateProduct;
using EcommerceDev.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand request)
        {
            var result = await
                _mediator.DispatchAsync<CreateProductCommand, ResultViewModel<Guid>>(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
