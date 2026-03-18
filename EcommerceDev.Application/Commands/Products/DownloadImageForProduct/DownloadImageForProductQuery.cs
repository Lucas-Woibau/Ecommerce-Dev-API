namespace EcommerceDev.Application.Commands.Products.DownloadImageForProduct
{
    public class DownloadImageForProductQuery
    {
        public DownloadImageForProductQuery(Guid idProductImage)
        {
            IdProductImage = idProductImage;
        }

        public Guid IdProductImage { get; set; }
    }
}
