using DotnetApiPostgres.Api.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiPostgres.Api.Models;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<string> CreateAdminToken([FromBody] CreateAdminTokenCommand data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }
}