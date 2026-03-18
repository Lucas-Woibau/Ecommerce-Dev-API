namespace EcommerceDev.Application.Commands.Products.UploadImageForProductCommand
{
    public class UploadImageForProductCommand
    {
        public UploadImageForProductCommand(Guid idProduct, string fileName, MemoryStream imageStream)
        {
            IdProduct = idProduct;
            FileName = fileName;
            ImageStream = imageStream;
        }

        public Guid IdProduct { get; set; }
        public string FileName { get; set; }
        public MemoryStream ImageStream { get; set; }
    }
}
