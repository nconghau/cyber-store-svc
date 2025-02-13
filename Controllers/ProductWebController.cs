using CyberStoreSVC.Mediator.Queries.Web;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CyberStoreSVC.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductWebController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductWebController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<JsonResponse<List<ProductDTO>>> GetAllProduct([FromBody] GetAllProductQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

    [HttpPost]
    public async Task<PostgresDataSource<Product>> GetProductByQuery([FromBody] PostgresQuery query)
    {
        var jsonResponse = await _mediator.Send(new GetProductByQuery()
        {
            Query = query
        });
        return jsonResponse;
    }

    [HttpPost]
    public async Task<JsonResponse<ProductDTO>> GetProductByField([FromBody] GetProductByFieldQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

}