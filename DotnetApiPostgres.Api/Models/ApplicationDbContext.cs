using DotnetApiPostgres.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiPostgres.Api;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Dynamically register entities
        foreach (var entityType in DynamicEntityRegistry.GetEntities())
        {
            modelBuilder.Entity(entityType);
        }
    }

    public static class DynamicEntityRegistry
    {
        private static readonly List<Type> _entities = new();

        public static void AddEntity<T>() where T : class
        {
            _entities.Add(typeof(T));
        }

        public static IEnumerable<Type> GetEntities()
        {
            return _entities;
        }
    }
}