using CyberStoreSVC.Mediator.Queries.Web;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CyberStoreSVC.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryWebController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryWebController(IMediator mediator)
    {
        _mediator = mediator;
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