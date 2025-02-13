using System.Text.Json;
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
public class ProductAdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductAdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<JsonResponse<List<Product>>> Init()
    {
        var initData = DataConvert.ReadFromFile<ProductDTO>("File/products.json");

        var jsonResponse = await _mediator.Send(new InitProductCommand()
        {
            Datas = initData
        });

        return jsonResponse;
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