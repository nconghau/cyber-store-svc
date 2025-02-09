using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace CyberStoreSVC.Auth
{
    public class AuthAttribute : TypeFilterAttribute
    {
        public AuthAttribute(string roles) : base(typeof(AuthFilter))
        {
            Arguments = new object[] { roles };  
        }
    }

    public class AuthFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly string _roles; 

        public AuthFilter(IConfiguration configuration, string roles)
        {
            _configuration = configuration;
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new JsonResult(new { errorCode = "401" }) { StatusCode = 200 };
                return;
            }

            var token = authHeader.Substring("Bearer ".Length);
            var key = Encoding.ASCII.GetBytes($"AuthFilterToken::{_configuration["AuthFilterToken"]}");

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = "roles", 
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                var jwtToken = securityToken as JwtSecurityToken;

                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Result = new JsonResult(new { errorCode = "401" }) { StatusCode = 200 };
                    return;
                }

                var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == "r")?.Value;
                if (string.IsNullOrEmpty(roleClaim))
                {
                    context.Result = new JsonResult(new { errorCode = "403" }) { StatusCode = 200 };
                    return;
                }

                var allowedRoles = _roles.Split(',').Select(r => r.Trim()).ToArray();
                if (!allowedRoles.Any(role => roleClaim.Split(',').Contains(role)))
                {
                    context.Result = new JsonResult(new { errorCode = "403" }) { StatusCode = 200 };
                    return;
                }
            }
            catch
            {
                context.Result = new JsonResult(new { errorCode = "401" }) { StatusCode = 200 };
            }
        }
    }
}

