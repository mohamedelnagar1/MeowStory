using MeowStory.Application.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Infrastructure.Authentication
{
    public class JWTService : IJWTService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenOptions _token;

        public JWTService(UserManager<ApplicationUser> userManager,
            IOptions<TokenOptions> tokenOptions)
        {
            _userManager = userManager;
            _token = tokenOptions.Value;
        }

        public async Task<string> GenerateTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException();

            string role = (await _userManager.GetRolesAsync(user))[0];
            byte[] secret = Encoding.ASCII.GetBytes(_token.Secret);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _token.Issuer,
                Audience = _token.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id),
                    new Claim("Username", user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_token.AccessExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}