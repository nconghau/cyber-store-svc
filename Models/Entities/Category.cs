using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotnetApiPostgres.Api.Models.DTOs;

namespace DotnetApiPostgres.Api.Models.Entities;

[Table("Category")]
public class Category
{
    [Key]
    [Column(TypeName = "varchar(24)")] 
    [Required]
    public required string Id { get; set; }

    [Column(TypeName = "varchar(50)")]
    [Required]
    public required string Name { get; set; }

    [Column(TypeName = "varchar(255)")]
    [Required]
    public required string IconUrl { get; set; }

    public static CategoryDTO ToGetCategoryDto(Category category)
    {
        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            IconUrl = category.IconUrl
        };
    }
}