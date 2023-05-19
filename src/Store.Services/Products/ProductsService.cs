using Microsoft.EntityFrameworkCore;
using Store.Web.Data;

namespace Store.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;

        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductPaginationModel> All(int pageNumber = 1, int pageSize = 12)
        {
            int totalItems = _context.Products.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;
            }

            var products = await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    ImageURL = x.ImageURL,
                    CategoryName = x.ProductCategories.FirstOrDefault() == null ? string.Empty : x.ProductCategories.First().Category.Name,
                })
                .ToListAsync();

            return new ProductPaginationModel
            {
                Products = products,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };
        }
    }
}
