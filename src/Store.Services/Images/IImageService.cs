using Microsoft.AspNetCore.Http;

namespace Store.Services.Images
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile image, string productName);

        Task<string> SaveImageFromWeb(string imageUrl, string productName);
    }
}
