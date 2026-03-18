using EcommerceDev.Application.Commands.Products.DownloadImageForProduct;
using EcommerceDev.Application.Common;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Storage;

namespace EcommerceDev.Application.Commands.Products.DownloadAllImagesForProduct
{

    public class DownloadAllImagesForProductQuery
    {
        public Guid IdProduct { get; set; }

        public DownloadAllImagesForProductQuery(Guid idProduct)
        {
            IdProduct = idProduct;
        }
    }
    public class DownloadAllImagesForProductQueryHandler : IHandler<DownloadAllImagesForProductQuery, ResultViewModel<List<Stream>>>
    {
        private readonly IStorageService _storageService;

        public DownloadAllImagesForProductQueryHandler(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<ResultViewModel<List<Stream>>> HandleAsync(DownloadAllImagesForProductQuery request)
        {
            var streams = await _storageService.DonwloadImages($"{request.IdProduct}/");

            return ResultViewModel<List<Stream>>.Success(streams);
        }
    }
}
