using Store.Data;
using Store.Services.Products;

namespace Store.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ProductImporter>();
            services.AddHttpClient<ProductImporter>();
            services.AddScoped<DataSeeder>();

            services.AddTransient<IProductsService, ProductsService>();

            return services;
        }
    }
}
