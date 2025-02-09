using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CyberStoreSVC.Models.Entities;

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

    [Column(TypeName = "varchar(50)")]
    public string Phone { get; set; }

    [Required]
    public long OrderDate { get; set; }

    [Required]
    public string OrderAddress { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    [Required]
    public decimal TotalAmount { get; set; }

    // Ref OrderItem Many to
    public List<OrderItem> OrderItems { get; set; }
}

[Table("OrderItem")]
public class OrderItem
{
    [Key]
    [Column(TypeName = "varchar(24)")]
    [Required]
    public required string Id { get; set; }

    [Required]
    public string ProductId { get; set; }

    [Required]
    public string ProductName { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Qty { get; set; }

    // Ref Order
    [Required]
    [Column(TypeName = "varchar(24)")]
    public required string OrderId { get; set; }

    [ForeignKey("OrderId")]
    [JsonIgnore]
    public Order Order { get; set; }
}
