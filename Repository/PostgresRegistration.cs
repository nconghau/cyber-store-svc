using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CyberStoreSVC.Repository
{
    public static class PostgresRegistration
	{
        private static readonly string _postgresConnectionString = "Host=14.225.204.163;Port=5332;Database=cyber_store;Username=cyber_store;Password=cyber_store";
        public static IServiceCollection AddPostgresServices(this IServiceCollection services, IConfiguration configuration, bool scriptMigrationPostgres=false)
        {
            string postgresConnection = configuration["PostgresConnection"] ?? "";
            if (scriptMigrationPostgres)
            {
                postgresConnection = _postgresConnectionString;
            }

            using var connection = new NpgsqlConnection(postgresConnection);
            connection.Open();
            Console.WriteLine("PostgreSQL Connection successful!");
            Console.WriteLine($"PostgreSQL version: {connection.PostgreSqlVersion}");

            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(postgresConnection));

            services.AddTransient(typeof(IPostgresRepository<,>), typeof(PostgresRepository<,>));

            return services;
        }
    }
}

