using System.Diagnostics;
using System.Text.Json;
using CyberStoreSVC.Utils;
using MediatR;

namespace BuildingBlocks.Application.Behaviors
{
    public sealed class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly string _userName;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
        private readonly string _controllerName;
        private readonly string _actionName;
        private readonly string _userAgent;

        public LoggingBehaviour(IHttpContextAccessor httpContextAccessor, ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userName = httpContextAccessor.HttpContext?.GetRouteValue("userName")?.ToString() ?? "";
            _controllerName = httpContextAccessor.HttpContext?.GetRouteValue("controller")?.ToString() ?? "";
            _actionName = httpContextAccessor.HttpContext?.GetRouteValue("action")?.ToString() ?? "";
            _userAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown";
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Request
            var start = Stopwatch.GetTimestamp();
            var messageId = Guid.NewGuid().ToString();
            var response = await next();
            try
            {
                // Response
                var latency = $"{Math.Round((Stopwatch.GetTimestamp() - start) * 1000 / (double)Stopwatch.Frequency)}ms";
                try
                {
                    _logger.LogInformation(JsonSerializer.Serialize(new
                    {
                        time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        controller = _controllerName,
                        action = _actionName,
                        mediator = $"{typeof(TRequest).Name}",
                        userName = _userName,
                        responseSuccess = ((dynamic)response).Success ?? ((dynamic)response).success,
                        responseErrorCode = ((dynamic)response).ErrorCode ?? ((dynamic)response).errorCode,
                        request,
                        messageId,
                        name = "mediator-logs",
                        userAgent = _userAgent, 
                        env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                        port = _httpContextAccessor.HttpContext?.Connection.LocalPort,
                        latency
                    }, JsonSerializerOptionCommon.Create()));
                }
                catch (Exception)
                {
                    _logger.LogInformation(JsonSerializer.Serialize(new
                    {
                        time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        controller = _controllerName,
                        action = _actionName,
                        mediator = $"{typeof(TRequest).Name}",
                        userName = _userName,
                        request,
                        messageId,
                        name = "mediator-logs",
                        userAgent = _userAgent, 
                        env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                        port = _httpContextAccessor.HttpContext?.Connection.LocalPort,
                        latency
                    }, JsonSerializerOptionCommon.Create()));
                }

                if (_httpContextAccessor.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Response.Headers["latency"] = latency;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error occurred while logging request.");
            }

            return response;
        }
    }
}