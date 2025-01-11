using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotnetApiPostgres.Api.Models.DTOs;

namespace DotnetApiPostgres.Api.Models.Entities;

[Table("Person")]
public class Person
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(30)")]
    [Required]
    public required string Name { get; set; }

    public static GetPersonDTO ToGetPersonDto(Person person)
    {
        return new GetPersonDTO
        {
            Id = person.Id,
            Name = person.Name
        };
    }
}