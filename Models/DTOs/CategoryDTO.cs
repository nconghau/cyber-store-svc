namespace DotnetApiPostgres.Api.Models.DTOs;

public class CategoryDto
{
    public string Id { get; set; }
    public required string Name { get; set; }
    public required string IconUrl { get; set; }
}