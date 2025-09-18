using Google.Cloud.Diagnostics.AspNetCore3;
using Google.Cloud.Diagnostics.Common;
using Google.Cloud.Logging.V2;
using Google.Cloud.Trace.V1;

namespace CyberStoreSVC.Services.GoogleServices
{
    public static class GoogleServicesRegistration
	{
        private static readonly string _credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "Private", "cyber_store_gcp_logs.json");
        private static readonly string _projectId = "project_id";
        private static readonly string _serviceName = "cyber_store_svc";

        public static IServiceCollection AddGoogleDiagnosticsServices(this IServiceCollection services)
        {
            services.AddGoogleDiagnosticsForAspNetCore(new AspNetCoreTraceOptions()
            {
                ServiceOptions = new TraceServiceOptions()
                {
                    ProjectId = _projectId,
                    Client = new TraceServiceClientBuilder()
                    {
                        CredentialsPath = _credentialsPath
                    }.Build(),
                },
            }, new LoggingServiceOptions()
            {
                ProjectId = _projectId,
                ServiceName = _serviceName,
                Client = new LoggingServiceV2ClientBuilder()
                {
                    CredentialsPath = _credentialsPath
                }.Build(),
            }, new ErrorReportingServiceOptions()
            {
                ProjectId = _projectId,
                ServiceName = _serviceName,
                Client = new LoggingServiceV2ClientBuilder()
                {
                    CredentialsPath = _credentialsPath
                }.Build(),
            });

            return services;
        }
    }
}

