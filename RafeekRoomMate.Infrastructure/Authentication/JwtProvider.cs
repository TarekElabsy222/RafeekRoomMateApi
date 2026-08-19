using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RafeekRoomMate.Infrastructure.Authentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        #region fields
        private readonly JwtOptions _options;
        #endregion

        #region constructor
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        #endregion

        #region handle functions        
        public string GenerateAccessToken(TokenUser user)
        {
            var expiresAt = DateTime.UtcNow .AddMinutes(_options.AccessTokenExpirationMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new( JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(_options.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public DateTime GetAccessTokenExpiration()
        {
            return DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes);
        }
        #endregion
    }
}
