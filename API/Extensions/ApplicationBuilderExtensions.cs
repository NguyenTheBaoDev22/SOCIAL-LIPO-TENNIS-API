using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task MigrateAndSeedAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
            await DbSeeder.SeedAsync(dbContext);
        }
    }
}
