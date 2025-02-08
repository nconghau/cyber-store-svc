using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApiPostgres.Api.Models.Entities;

[Table("Product")]
public class Product
{
    [Key]
    [Column(TypeName = "varchar(24)")] 
    [Required]
    public required string Id { get; set; }

    [Column(TypeName = "varchar(255)")]
    [Required]
    public required string Name { get; set; }

    [Column(TypeName = "varchar(255)")]
    [Required]
    public required string ImgUrl { get; set; }

    [Column(TypeName = "json")]
    public required string ImgUrls { get; set; } = "[]";

    [Column(TypeName = "decimal(10, 2)")]
    [Required]
    public required decimal Price { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    [Required]
    public required decimal DiscountedPrice { get; set; }

    [Column(TypeName = "json")]
    public string Tags { get; set; } = "[]";

    [Column(TypeName = "varchar(50)")]
    [Required]
    public required string Brand { get; set; }

    [Column(TypeName = "text")]
    public string ShortDescription { get; set; } = string.Empty;

    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "json")]
    public string Properties { get; set; } = "[]"; // search Properties + Redis
}