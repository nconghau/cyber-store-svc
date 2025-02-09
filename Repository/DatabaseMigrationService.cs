using Microsoft.EntityFrameworkCore;

namespace CyberStoreSVC.Repository
{
    public static class DatabaseMigrationService
    {
        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}

