using EcommerceDev.Application.Common;
using EcommerceDev.Core.Repositories;

namespace EcommerceDev.Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommandHandler : IHandler<CreateCategoryCommand, ResultViewModel<Guid>>
    {
        private readonly IMediator _mediator;
        private readonly IProductCategoryRepository _repository;

        public CreateCategoryCommandHandler(IMediator mediator, IProductCategoryRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ResultViewModel<Guid>> HandleAsync(CreateCategoryCommand request)
        {
            var category = new ProductCategory(request.Title, request.Subcategory);

            await _repository.Create(category);

            return ResultViewModel<Guid>.Success(category.Id);
        }
    }
}
