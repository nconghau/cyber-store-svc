using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;

namespace CyberStoreSVC.Mappings;

public static class CategoryMappings
{
    public static CategoryDTO ToCategoryDto(this Category entity)
    {
        return new CategoryDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            IconUrl = entity.IconUrl
        };
    }

    public static Category ToCategory(this CategoryDTO dto)
    {
        return new Category
        {
            Id = dto.Id,
            Name = dto.Name,
            IconUrl = dto.IconUrl
        };
    }
}
