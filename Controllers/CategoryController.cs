using DotnetApiPostgres.Api.Mediator.Commands.Admin;
using DotnetApiPostgres.Api.Mediator.Queries.Web;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiPostgres.Api.Models;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<JsonResponse<List<Category>>> Init()
    {
        var initData = new List<CategoryDto>()
        {
            new CategoryDto()
            {
                Id=IdGenerator.GenerateId(),
                Name="Phones",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360178/cyber_store_project/Phones_aytd5e.svg",
            },
            new CategoryDto()
            {
                Id=IdGenerator.GenerateId(),
                Name="Smart Watches",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360178/cyber_store_project/Smart_Watches_fbk69c.svg",
            },
             new CategoryDto()
            {
                Id=IdGenerator.GenerateId(),
                Name="Cameras",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360177/cyber_store_project/Cameras_hsypos.svg",
            },
              new CategoryDto()
            {
                Id=IdGenerator.GenerateId(),
                Name="Headphones",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360178/cyber_store_project/Headphones_pajmi6.svg",
            },
               new CategoryDto()
            {
                Id=IdGenerator.GenerateId(),
                Name="Computers",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360553/cyber_store_project/Computers_jyvftv.svg",
            },
                new CategoryDto()
            {
                Id=IdGenerator.GenerateId(),
                Name="Gaming",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360178/cyber_store_project/Gaming_r8you9.svg",
            }
        };

        var jsonResponse = await _mediator.Send(new InitCateroryCommand()
        {
            Datas = initData
        });
        return jsonResponse;
    }

    [HttpPost]
    public async Task<JsonResponse<List<CategoryDto>>> GetAllCategory([FromBody] GetAllCategoryQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

    [HttpPost]
    public async Task<PostgresDataSource<Category>> GetCategoryByQuery([FromBody] GetCategoryByQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

}