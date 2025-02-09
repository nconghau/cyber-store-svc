using System.Reflection;
using System.Text;
using BuildingBlocks.Application.Behaviors;
using CyberStoreSVC.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CyberStoreSVC.Services.Common
{
    public static class CommonRegistration
	{
        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes($"AuthFilterToken::{configuration["AuthFilterToken"]}");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(op =>
            {
                op.OperationFilter<AuthHeaderOperatorFilter>();
                op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                op.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
            });
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

