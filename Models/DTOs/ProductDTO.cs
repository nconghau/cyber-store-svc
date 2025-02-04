namespace DotnetApiPostgres.Api.Models.DTOs;

public class ProductDTO
{
    public string Id { get; set; }
    public required string Name { get; set; }
    public required string ImgUrl { get; set; }
    public string ImgUrls { get; set; } = "[]";
    public required decimal Price { get; set; }
    public required decimal DiscountedPrice { get; set; }
    public string Tags { get; set; } = "[]";
    public required string Brand { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Properties { get; set; } = "[]";
}