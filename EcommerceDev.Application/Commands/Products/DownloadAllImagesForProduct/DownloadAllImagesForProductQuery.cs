using EcommerceDev.Application.Commands.Products.DownloadImageForProduct;
using EcommerceDev.Core.Repositories;

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
}
