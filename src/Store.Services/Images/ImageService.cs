using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Store.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly IHostEnvironment _environment;

        public ImageService(HttpClient httpClient, IHostEnvironment environment)
        {
            _httpClient = httpClient;
            _environment = environment;
        }

        public async Task<string> SaveImage(IFormFile image, string productName)
        {
            if (image == null)
            {
                return @"/img/placeholder.jpg";
            }

            var fileExtension = Path.GetExtension(image.FileName);
            var filePath = GenerateFilePath(productName, fileExtension);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var path = "/img/" + Path.GetFileName(filePath);
            return path;
        }

        public async Task<string> SaveImageFromWeb(string imageUrl, string productName)
        {
            if (imageUrl == null)
            {
                return @"/img/placeholder.jpg";
            }

            var response = await _httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();

            var fileExtension = Path.GetExtension(new Uri(imageUrl).AbsolutePath);
            var filePath = GenerateFilePath(productName, fileExtension);

            await File.WriteAllBytesAsync(filePath, bytes);

            var path = "/img/" + Path.GetFileName(filePath);
            return path;
        }

        private string GenerateFilePath(string productName, string fileExtension)
        {
            var fileName = productName.ToLower().Replace(" ", "-").Replace("/", "-") + fileExtension;
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", "img", fileName);
            return filePath;
        }
    }
}
