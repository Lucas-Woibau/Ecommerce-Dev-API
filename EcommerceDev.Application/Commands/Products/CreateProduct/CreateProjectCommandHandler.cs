using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;

namespace EcommerceDev.Application.Commands.Products.CreateProduct
{
    public class CreateProjectCommandHandler : IHandler<CreateProductCommand, ResultViewModel<Guid>>
    {
        private readonly IProductRepository _repository;

        public CreateProjectCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<Guid>> HandleAsync(CreateProductCommand request)
        {
            var product = new Product(
                request.Title,
                request.Description,
                request.Price,
                request.Brand,
                request.Quantity,
                request.IdCategory
                );

            await _repository.Create(product);

            return ResultViewModel<Guid>.Success(product.Id);
        }
    }
}
