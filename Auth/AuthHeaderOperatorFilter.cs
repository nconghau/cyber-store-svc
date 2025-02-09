using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DotnetApiPostgres.Api.Auth
{
    public sealed class AuthHeaderOperatorFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!string.IsNullOrEmpty(context.ApiDescription.RelativePath) && !context.ApiDescription.RelativePath.Contains("admin"))
            {
                return;
            }
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            context.ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo);
            var allowAnonymousAttributes = methodInfo.CustomAttributes.Where(w => w.AttributeType == typeof(AllowAnonymousAttribute)).FirstOrDefault();
            if (allowAnonymousAttributes != null)
            {
                return;
            }

            operation.Summary += " (Need authorization for the request)";

            var authorization = new OpenApiParameter
            {
                Name = "authorization",
                In = ParameterLocation.Header,
                Description = "Authorization",
                Required = false,
            };
            operation.Parameters.Add(authorization);

            return;
        }
    }
}

