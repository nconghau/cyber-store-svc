using CyberStoreSVC.Auth;
using CyberStoreSVC.Mediator.Commands.Admin;
using CyberStoreSVC.Mediator.Queries.Web;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CyberStoreSVC.Models;

[ApiController]
[Route("api/admin/[controller]/[action]")]
[Auth("ADMIN")]
public class CategoryAdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryAdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<JsonResponse<List<Category>>> Init()
    {
        var initData = new List<CategoryDTO>()
        {
            new CategoryDTO()
            {
                Id=IdGenerator.GenerateId(),
                Name="Phones",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360178/cyber_store_project/Phones_aytd5e.svg",
            },
            new CategoryDTO()
            {
                Id=IdGenerator.GenerateId(),
                Name="Smart Watches",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360178/cyber_store_project/Smart_Watches_fbk69c.svg",
            },
             new CategoryDTO()
            {
                Id=IdGenerator.GenerateId(),
                Name="Cameras",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360177/cyber_store_project/Cameras_hsypos.svg",
            },
              new CategoryDTO()
            {
                Id=IdGenerator.GenerateId(),
                Name="Headphones",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360178/cyber_store_project/Headphones_pajmi6.svg",
            },
               new CategoryDTO()
            {
                Id=IdGenerator.GenerateId(),
                Name="Computers",
                IconUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360553/cyber_store_project/Computers_jyvftv.svg",
            },
                new CategoryDTO()
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
    public async Task<JsonResponse<List<CategoryDTO>>> GetAllCategory([FromBody] GetAllCategoryQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

    [HttpPost]
    public async Task<PostgresDataSource<Category>> GetCategoryByQuery([FromBody] PostgresQuery query)
    {
        var jsonResponse = await _mediator.Send(new GetCategoryByQuery()
        {
            Query = query
        });
        return jsonResponse;
    }

}