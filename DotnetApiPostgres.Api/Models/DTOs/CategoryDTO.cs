using DotnetApiPostgres.Api.Models.Entities;

namespace DotnetApiPostgres.Api.Models.DTOs;

public class CategoryDto
{
    public string Id { get; set; }
    public required string Name { get; set; }
    public required string IconUrl { get; set; }

    public static Category ToCategory(CategoryDto categoryDto)
    {
        return new Category
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            IconUrl = categoryDto.IconUrl
        };
    }
}