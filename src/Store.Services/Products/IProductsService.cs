using Store.Services.Products.Models;

namespace Store.Services.Products
{
    public interface IProductsService
    {
        Task<ProductModel> GetById(int productId);

        Task<List<ProductModel>> GetRelatedProducts(int productId, string categoryName, int? count);

        Task<ProductListingModel> GetFilteredProducts(int pageNumber = 1, int pageSize = 12, string category = null);

        Task<List<string>> GetAllCategories();
    }
}
