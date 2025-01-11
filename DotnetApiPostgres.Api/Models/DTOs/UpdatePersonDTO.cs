using System.ComponentModel.DataAnnotations;
using DotnetApiPostgres.Api.Models.Entities;

namespace DotnetApiPostgres.Api.Models.DTOs;

public class UpdatePersonDTO
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }

    public static Person ToPerson(UpdatePersonDTO updatePersonDto)
    {
        return new Person
        {
            Id = updatePersonDto.Id,
            Name = updatePersonDto.Name
        };
    }
}