using RafeekRoomMate.Application.Common.Models;

namespace RafeekRoomMate.Application.Common.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(TokenUser user);

        DateTime GetAccessTokenExpiration();
    }
}
