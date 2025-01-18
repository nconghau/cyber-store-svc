using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using DotnetApiPostgres.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiPostgres.Api.Models;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly IPostgresRepository<Category, string> _repository;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(IPostgresRepository<Category, string> repository, ILogger<CategoryController> logger)
    {
        _repository = repository;
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
        var initDataResult = new List<Category>();

        await _repository.DeleteAsync(f => true);

        foreach (var data in initData)
        {
            var category = await _repository.AddAsync(data.ToCategory());
            if (category != null)
            {
                initDataResult.Add(category);
            }
        }
        return new JsonResponse<List<Category>>(true, initDataResult);
    }

    [HttpGet]
    public async Task<JsonResponse<List<CategoryDto>>> GetAllCategory()
    {
        var categories = await _repository.GetAllAsync();
        var response = new JsonResponse<List<CategoryDto>>(true, categories.Select(p => p.ToCategoryDto()).ToList());
        return response;
    }

    [HttpPost]
    public async Task<PostgresDataSource<Category>> GetCategoryByQuery([FromBody] PostgresQuery query)
    {
        var response = await _repository.GetByQueryAsync(query);
        return response;
    }

}