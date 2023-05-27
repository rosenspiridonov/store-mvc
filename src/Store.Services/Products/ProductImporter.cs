using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using Store.Data.Entities;
using Store.Services.Products.Models;
using Store.Web.Data;

using System.Text.Json;

namespace Store.Services.Products
{
    public class ProductImporter
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _environment;

        public ProductImporter(HttpClient httpClient, ApplicationDbContext context, IHostEnvironment environment)
        {
            _httpClient = httpClient;
            _context = context;
            _environment = environment;
        }

        public async Task ImportProductsAsync()
        {
            var productsCount = await _context.Products.CountAsync();

            if (productsCount > 0)
            {
                return;
            }

            var response = await _httpClient.GetAsync("https://dummyjson.com/products?limit=100");

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var productResponse = JsonSerializer.Deserialize<ProductResponse>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new ProductResponse();
            var products = productResponse.Products;

            var addedCategories = new List<string>();

            foreach (var productDto in products)
            {
                var product = new Product
                {
                    Name = productDto.Title,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    ImageURL = await SaveImage(productDto.Images.FirstOrDefault(), productDto.Title)
                };

                var categoryName = CapitalizeFirstLetter(productDto.Category);
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName) ?? new Category { Name = categoryName };

                product.ProductCategories.Add(new ProductCategory { Category = category });

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<string> SaveImage(string imageUrl, string productName)
        {
            if (imageUrl == null)
            {
                return @"/img/placeholder.jpg";
            }

            var response = await _httpClient.GetAsync(imageUrl);

            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();

            var fileExtension = Path.GetExtension(new Uri(imageUrl).AbsolutePath);
            var fileName = productName.ToLower().Replace(" ", "-").Replace("/", "-") + fileExtension;

            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", "img", fileName);

            await File.WriteAllBytesAsync(filePath, bytes);

            var path = "/img/" + fileName;

            return path;
        }

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }
    }
}
