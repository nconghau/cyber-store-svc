using System.ComponentModel.DataAnnotations;
using DotnetApiPostgres.Api.Models.Entities;

namespace DotnetApiPostgres.Api.Models.DTOs;

public class CreatePersonDTO
{
    [Required]
    public required string Name { get; set; }

    public static Person ToPerson(CreatePersonDTO createPersonDto)
    {
        return new Person
        {
            Name = createPersonDto.Name
        };
    }
}