namespace RafeekRoomMate.Application.Features.Authentication.Commands.RevokeRefreshToken
{
    public sealed class RevokeRefreshTokenCommand
    {
        public string RefreshToken { get; init; } = string.Empty;
    }
}
