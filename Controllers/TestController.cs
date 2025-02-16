using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace CyberStoreSVC.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;
    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<string> Ping()
    {
        return "Pong pong!";
    }
}