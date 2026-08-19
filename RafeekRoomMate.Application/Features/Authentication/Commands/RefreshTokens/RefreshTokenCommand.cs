namespace RafeekRoomMate.Application.Features.Authentication.Commands.RefreshTokens
{
    public sealed class RefreshTokenCommand
    {
        public string RefreshToken { get; init; } = string.Empty;
    }
}
