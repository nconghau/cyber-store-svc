using CyberStoreSVC.Auth;
using CyberStoreSVC.Mediator.Commands.Admin;
using CyberStoreSVC.Mediator.Queries.Web;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CyberStoreSVC.Controllers;

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
        var initData = DataConvert.ReadFromFile<CategoryDTO>("File/categories.json");

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