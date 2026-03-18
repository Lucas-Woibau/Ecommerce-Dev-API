using EcommerceDev.Application.Common;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Storage;

namespace EcommerceDev.Application.Commands.Products.UploadImageForProductCommand
{
    public class UploadImageForProductCommandHandler : IHandler<UploadImageForProductCommand, ResultViewModel<bool>>
    {
        private readonly IStorageService _storageService;
        private readonly IProductRepository _productRepository;

        public UploadImageForProductCommandHandler(IStorageService storageService, IProductRepository productRepository)
        {
            _storageService = storageService;
            _productRepository = productRepository;
        }

        public async Task<ResultViewModel<bool>> HandleAsync(UploadImageForProductCommand request)
        {
            var extension = request.FileName.Split('.').Last();

            var productImage = new ProductImage(true, request.IdProduct);
            productImage.ConfigureIdentifier(extension);

            await _storageService.UploadImage(productImage.Path,request.ImageStream);

            await _productRepository.CreateImage(productImage);
            return ResultViewModel<bool>.Success(true);
        }
    }
}
