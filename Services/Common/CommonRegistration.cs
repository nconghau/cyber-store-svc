using System.Reflection;
using BuildingBlocks.Application.Behaviors;
using MediatR;

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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            return services;
        }
    }
}

