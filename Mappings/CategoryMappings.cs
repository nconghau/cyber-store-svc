using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;

namespace DotnetApiPostgres.Api.Mappings;

public static class CategoryMappings
{
    public static CategoryDto ToCategoryDto(this Category entity)
    {
        return new CategoryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IconUrl = entity.IconUrl
        };
    }

    public static Category ToCategory(this CategoryDto dto)
    {
        return new Category
        {
            Id = dto.Id,
            Name = dto.Name,
            IconUrl = dto.IconUrl
        };
    }
}
