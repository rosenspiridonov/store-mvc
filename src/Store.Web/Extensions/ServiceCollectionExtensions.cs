using Store.Data;
using Store.Services.Cart;
using Store.Services.Categories;
using Store.Services.Images;
using Store.Services.Products;

namespace Store.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IImageService, ImageService>();
            services.AddTransient<ProductImporter>();
            services.AddHttpClient<ProductImporter>();
            services.AddScoped<DataSeeder>();

            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICartService, CartService>();

            return services;
        }
    }
}
