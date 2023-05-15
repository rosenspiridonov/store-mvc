using Microsoft.AspNetCore.Builder;

using Store.Data;

namespace Store.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<DataSeeder>();

            return services;
        }
    }
}
