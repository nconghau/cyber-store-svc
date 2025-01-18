using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;

namespace DotnetApiPostgres.Api.Mappings;

public static class CategoryMappings
{
    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            IconUrl = category.IconUrl
        };
    }

    public static Category ToCategory(this CategoryDto categoryDto)
    {
        return new Category
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            IconUrl = categoryDto.IconUrl
        };
    }
}
