namespace EcommerceDev.Infrastructure.Storage
{
    public interface IStorageService
    {
        Task<bool> UploadImage(string path, Stream fileStream);
        Task<Stream> DownloadImage(string path);
        Task<List<Stream>> DonwloadImages(string path);
    }
}
