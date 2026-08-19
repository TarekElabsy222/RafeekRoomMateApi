using Microsoft.Extensions.Options;
using RafeekRoomMate.Application.Common.Interfaces;
using System.Security.Cryptography;

namespace RafeekRoomMate.Infrastructure.Authentication
{
    public sealed class RefreshTokenProvider: IRefreshTokenProvider
    {
        #region fields
        private readonly JwtOptions _options;
        #endregion

        #region constructor
        public RefreshTokenProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        #endregion

        #region handle fuctions        
        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        public DateTime GetRefreshTokenExpiration()
        {
            return DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays);
        }
        #endregion
    }
}
