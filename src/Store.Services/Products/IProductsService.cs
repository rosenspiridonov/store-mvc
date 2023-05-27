namespace Store.Services.Products
{
    public interface IProductsService
    {
        Task<ProductListingModel> GetFilteredProducts(int pageNumber = 1, int pageSize = 12, string category = null);

        Task<List<string>> GetAllCategories();
    }
}
