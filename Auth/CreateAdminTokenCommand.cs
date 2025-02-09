using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CyberStoreSVC.Mediator.Common;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace CyberStoreSVC.Auth
{
    public sealed class CreateAdminTokenCommand : ICommand<string>
    {
        public required string UserName { get; set; }
    }

    public sealed class CreateAdminTokenCommandValidator : AbstractValidator<CreateAdminTokenCommand>
    {
        public CreateAdminTokenCommandValidator()
        {
            RuleFor(c => c.UserName).NotEmpty();
        }
    }

    public sealed class CreateAdminTokenCommandHandler : ICommandHandler<CreateAdminTokenCommand, string>
    {
        public async Task<string> Handle(CreateAdminTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes($"AuthFilterToken::{Environment.GetEnvironmentVariable("AuthFilterToken")}");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim("svc", "cyber_store_svc"),
                            new Claim("userName", request.UserName),
                            new Claim("r", "ADMIN,USER"),
                        }
                    ),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch
            {
                return "";
            }
        }

    }
}

