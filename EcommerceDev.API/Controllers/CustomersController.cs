using EcommerceDev.Application.Commands.Customers.CreateCustomer;
using EcommerceDev.Application.Commands.Customers.CreateCustomerAddress;
using EcommerceDev.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerCommand request)
        {
            var result = await
                _mediator.DispatchAsync<CreateCustomerCommand, ResultViewModel<Guid>>(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("{customerId}/addresses")]
        public async Task<IActionResult> CreateAddress(CreateCustomerAddressCommand request)
        {
            var result = await
                _mediator.DispatchAsync<CreateCustomerAddressCommand, ResultViewModel<Guid>>(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
