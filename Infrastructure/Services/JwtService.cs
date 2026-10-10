using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using hackatonxp.Application.Interfaces;
using hackatonxp.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace hackatonxp.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public AuthResponseDto GenerateToken(Guid advisorId, string email)
        {
            var secret = _config["Jwt:Secret"] ?? Environment.GetEnvironmentVariable("JWT_SECRET");
            var issuer = _config["Jwt:Issuer"] ?? "hackatonxp";
            var audience = _config["Jwt:Audience"] ?? "hackatonxp_clients";
            var expiryMinutes = int.TryParse(_config["Jwt:ExpiryMinutes"], out var m) ? m : 60;

            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("JWT secret not configured. Set Jwt:Secret or JWT_SECRET environment variable.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, advisorId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("role", "advisor")
            };

            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: creds);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto { Token = tokenStr, ExpiresAt = expires };
        }
    }
}
