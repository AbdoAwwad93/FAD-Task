using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
// using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FadTask.Services
{
    public class AuthService : IAuthService
    {
        private readonly Repositories.IUserRepository _userRepository;

        public AuthService(Repositories.IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string? Authenticate(string email, string password)
        {
            if (!_userRepository.ValidateCredentials(email, password))
                return null;

            var key = Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new InvalidOperationException("JWT Key not configured in environment");
            var issuer = Environment.GetEnvironmentVariable("Jwt__Issuer"); // optional
            var audience = Environment.GetEnvironmentVariable("Jwt__Audience"); // optional
            var expireMinutes = int.TryParse(Environment.GetEnvironmentVariable("Jwt__ExpireMinutes"), out var em) ? em : 60;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
