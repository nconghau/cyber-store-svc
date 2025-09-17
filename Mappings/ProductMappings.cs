using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;

namespace CyberStoreSVC.Mappings;

public static class ProductMappings
{
    public static ProductDTO ToProductDto(this Product entity)
    {
        return new ProductDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            ImgUrl = entity.ImgUrl,
            ImgUrls = entity.ImgUrls,
            Price = entity.Price,
            DiscountedPrice = entity.DiscountedPrice,
            Tags = entity.Tags,
            Brand = entity.Brand,
            ShortDescription = entity.ShortDescription,
            Description = entity.Description,
            Properties = entity.Properties,
        };
    }

    public static Product ToProduct(this ProductDTO dto)
    {
        return new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            ImgUrl = dto.ImgUrl,
            ImgUrls = dto.ImgUrls,
            Price = dto.Price,
            DiscountedPrice = dto.DiscountedPrice,
            Tags = dto.Tags,
            Brand = dto.Brand,
            ShortDescription = dto.ShortDescription,
            Description = dto.Description,
            Properties = dto.Properties,
        };
    }

    public static void UpdateWithDto(this Product entity, ProductDTO dto)
    {
        entity.Name = dto.Name;
        entity.ImgUrl = dto.ImgUrl;
        entity.ImgUrls = dto.ImgUrls;
        entity.Price = dto.Price;
        entity.DiscountedPrice = dto.DiscountedPrice;
        entity.Tags = dto.Tags;
        entity.Brand = dto.Brand;
        entity.ShortDescription = dto.ShortDescription;
        entity.Description = dto.Description;
        entity.Properties = dto.Properties;
    }

}
