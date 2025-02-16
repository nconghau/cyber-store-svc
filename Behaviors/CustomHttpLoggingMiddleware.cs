using System.Diagnostics;

namespace CyberStoreSVC.Behaviors
{
    public class CustomHttpLoggingHandler : DelegatingHandler
    {
        private readonly ILogger<CustomHttpLoggingHandler> _logger;

        public CustomHttpLoggingHandler(ILogger<CustomHttpLoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[HTTP OUT] {request.Method} {request.RequestUri}");

            var stopwatch = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            _logger.LogInformation($"[HTTP OUT] {response.StatusCode} {request.RequestUri} took {stopwatch.ElapsedMilliseconds}ms");

            return response;
        }
    }
}

