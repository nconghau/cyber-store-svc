using DotnetApiPostgres.Api.Models.Entities;

namespace DotnetApiPostgres.Api.Models.DTOs;

public class GetPersonDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public static Person ToPerson(GetPersonDTO getPersonDto)
    {
        return new Person
        {
            Id = getPersonDto.Id,
            Name = getPersonDto.Name
        };
    }
}