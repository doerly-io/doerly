using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.DataAccess.Utils;

public static class DbMigrator
{
    public static void MigrateDatabase<TDbContext>(this IServiceProvider serviceProvider) where TDbContext : DbContext
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            context.Database.Migrate();
        }
    }
}
