using EcommerceDev.Application.Commands.Categories.CreateCategory;
using EcommerceDev.Application.Commands.Products.CreateProduct;
using EcommerceDev.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommand request)
        {
            var result = await
                _mediator.DispatchAsync<CreateCategoryCommand, ResultViewModel<Guid>>(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
