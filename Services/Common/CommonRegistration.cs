using System.Reflection;

namespace DotnetApiPostgres.Api.Services.Common
{
    public static class CommonRegistration
	{
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddHttpClient();

            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            return services;
        }
    }
}

