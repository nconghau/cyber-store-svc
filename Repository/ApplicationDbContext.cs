using DotnetApiPostgres.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiPostgres.Api;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        //// Configure the one-to-one relationship between Order and Address
        //modelBuilder.Entity<Order>()
        //    .HasOne(o => o.ShippingAddress)  // One Order has one Address
        //    .WithOne(a => a.Order)           // Each Address is linked to one Order
        //    .HasForeignKey<Address>(a => a.OrderId); // Foreign key in Address

        //// Configure OrderItem's relationship with Order (one-to-many)
        //modelBuilder.Entity<OrderItem>()
        //    .HasOne(oi => oi.Order)
        //    .WithMany(o => o.Items)
        //    .HasForeignKey(oi => oi.OrderId);
    }

    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
}
