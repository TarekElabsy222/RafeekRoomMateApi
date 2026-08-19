namespace RafeekRoomMate.Application.Common.Models
{
    public class TokenUser
    {
        public Guid Id { get; init; }

        public string Email { get; init; } = string.Empty;

        public IReadOnlyCollection<string> Roles { get; init; } = Array.Empty<string>();
    }
}
