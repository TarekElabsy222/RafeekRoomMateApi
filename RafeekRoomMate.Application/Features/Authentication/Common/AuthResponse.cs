namespace RafeekRoomMate.Application.Features.Authentication.Common
{
    public class AuthResponse
    {
        public string AccessToken { get; init; } = string.Empty;

        public DateTime AccessTokenExpiresAt { get; init; }

        public string RefreshToken { get; init; } = string.Empty;

        public DateTime RefreshTokenExpiresAt { get; init; }

        public AuthUserDto User { get; init; } = default!;
        public bool IsProfileCompleted { get; set; }
    }
}
