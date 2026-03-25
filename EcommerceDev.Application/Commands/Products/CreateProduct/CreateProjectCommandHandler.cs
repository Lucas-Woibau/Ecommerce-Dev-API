using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Caching;

namespace EcommerceDev.Application.Commands.Products.CreateProduct
{
    public class CreateProjectCommandHandler : IHandler<CreateProductCommand, ResultViewModel<Guid>>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cacheService;

        public CreateProjectCommandHandler(IProductRepository repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
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

            await _cacheService.RemoveAsync("products:all");

            return ResultViewModel<Guid>.Success(product.Id);
        }
    }
}
