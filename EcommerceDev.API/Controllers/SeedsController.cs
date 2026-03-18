using EcommerceDev.Core.Entities;
using EcommerceDev.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers
{
    [ApiController]
    [Route("api/seeds")]
    public class SeedsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> SeedData(
            [FromServices] EcommerceDbContext context)
        {
            var category = new ProductCategory("Tecnologia", "Computador");

            var customer = new Customer(
                "Lucas Woibau", "lucaswoibau7@gmail.com", "27999599903", DateTime.Now.AddYears(-25), "12345678910");

            var customerAddress = new CustomerAddress(
                Guid.NewGuid(), customer.FullName, "Rua Exemplo, 123", "Apto 101", "29750-000", "Centro", "ES", "Brasil", "Pancas");

            var product = new Product(
                "Notebook Acer", "Um notebook para jogos", 6_000m, "Acer", 100, category.Id);

            var order = new Order(
                customer.Id, customerAddress.Id, 10, 6000, new List<OrderItem>
                {
                    new OrderItem(product.Id, 1, 6000)
                });

            var objects = new List<object>
            {
                category,
                customer,
                customerAddress,
                product,
                order
            };

            await context.AddRangeAsync(objects);
            await context.SaveChangesAsync();

            return Ok(product.Id);
        }
    }
}
