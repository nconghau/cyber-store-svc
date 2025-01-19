using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;

namespace DotnetApiPostgres.Api.Mappings;

public static class ProductMappings
{
    public static ProductDto ToProductDto(this Product entity)
    {
        return new ProductDto
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

    public static Product ToProduct(this ProductDto dto)
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
}
