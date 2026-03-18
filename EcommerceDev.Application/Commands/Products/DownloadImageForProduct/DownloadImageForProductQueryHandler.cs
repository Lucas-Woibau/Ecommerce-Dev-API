using EcommerceDev.Application.Common;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Storage;

namespace EcommerceDev.Application.Commands.Products.DownloadImageForProduct
{
    public class DownloadImageForProductQueryHandler : IHandler<DownloadImageForProductQuery, ResultViewModel<Stream>>
    {
        private readonly IStorageService _storageService;
        private readonly IProductRepository _productRepository;

        public DownloadImageForProductQueryHandler(IStorageService storageService, IProductRepository productRepository)
        {
            _storageService = storageService;
            _productRepository = productRepository;
        }

        public async Task<ResultViewModel<Stream>> HandleAsync(DownloadImageForProductQuery request)
        {
            var productImage = await _productRepository.GetImageById(request.IdProductImage);

            if (productImage == null)
            {
                return ResultViewModel<Stream>.Error("Imagem não encontrada");
            }

            var stream = await _storageService.DownloadImage(productImage.Path);

            return ResultViewModel<Stream>.Success(stream);
        }
    }
}
