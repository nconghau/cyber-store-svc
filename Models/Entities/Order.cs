using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApiPostgres.Api.Models.Entities;

[Table("Order")]
public class Order
{
    [Key]
    [Column(TypeName = "varchar(24)")]
    [Required]
    public required string Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    [Required]
    public string CustomerName { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; }
    [Column(TypeName = "decimal(10, 2)")]
    [Required]
    public decimal TotalAmount { get; set; }
    public long OrderDate { get; set; }

    // Navigation property for the related Address
    //public Address ShippingAddress { get; set; } // Assuming one-to-one relation
    //public ICollection<OrderItem> Items { get; set; }
}


//[Table("Address")]
public class Address
{
    public int AddressId { get; set; } // Primary key for Address
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    // Foreign key for the related order
    public string OrderId { get; set; } // Foreign key pointing to Order

    // Navigation property back to the Order
    public Order Order { get; set; }
}


//[Table("OrderItem")]
public class OrderItem
{
    public string OrderItemId { get; set; } // Primary key for OrderItem
    public string OrderId { get; set; } // Foreign key pointing to Order
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // Navigation property back to the Order
    public Order Order { get; set; }
}
