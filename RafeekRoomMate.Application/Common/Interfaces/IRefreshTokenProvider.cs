namespace RafeekRoomMate.Application.Common.Interfaces
{
    public interface IRefreshTokenProvider
    {
        string GenerateRefreshToken();

        DateTime GetRefreshTokenExpiration();
    }
}
