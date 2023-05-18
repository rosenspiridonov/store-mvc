using Microsoft.EntityFrameworkCore;

using Store.Data;
using Store.Services;

namespace Store.Web.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task MigrateDatabaseAsync<TContext>(this WebApplication app) where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            await dbContext.Database.MigrateAsync();
        }

        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dataSeeder = services.GetRequiredService<DataSeeder>();
                var productImporter = services.GetRequiredService<ProductImporter>();

                await dataSeeder.SeedDataAsync();
                await productImporter.ImportProductsAsync();
            }
        }
    }
}
