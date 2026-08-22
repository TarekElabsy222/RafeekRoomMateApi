namespace RafeekRoomMate.Application.Features.Rooms.DTOs.GetDetailsRoom
{
    public sealed class RoommateDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Bio { get; init; } = string.Empty;

        public string? ProfileImageKey { get; init; }

        public DateTime JoinedAt { get; init; }
    }
}
