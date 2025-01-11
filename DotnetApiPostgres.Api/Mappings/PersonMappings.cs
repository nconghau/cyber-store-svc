using DotnetApiPostgres.Api.Models;
using DotnetApiPostgres.Api.Models.DTO;

namespace DotnetApiPostgres.Api.Mappings;

public static class PersonMappings
{
    public static GetPersonDto ToGetPersonDto(this Person person)
    {
        return new GetPersonDto
        {
            Id = person.Id,
            Name = person.Name
        };
    }

    public static Person ToPerson(this CreatePersonDTO dto)
    {
        return new Person
        {
            Name = dto.Name
        };
    }

    public static Person ToPerson(this UpdatePersonDTO dto)
    {
        return new Person
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }

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
