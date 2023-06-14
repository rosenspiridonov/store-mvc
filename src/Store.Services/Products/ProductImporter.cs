using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using Store.Data.Entities;
using Store.Services.Products.Models;
using Store.Data;

using System.Text.Json;
using Store.Services.Images;

namespace Store.Services.Products
{
    public class ProductImporter
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ProductImporter(HttpClient httpClient, ApplicationDbContext context, IImageService imageService)
        {
            _httpClient = httpClient;
            _context = context;
            _imageService = imageService;
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
                    ImageURL = await _imageService.SaveImageFromWeb(productDto.Images.FirstOrDefault(), productDto.Title)
                };

                var categoryName = CapitalizeFirstLetter(productDto.Category);
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.Name == categoryName) ?? new Category { Name = categoryName };

                product.ProductCategories.Add(new ProductCategory { Category = category });

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
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
