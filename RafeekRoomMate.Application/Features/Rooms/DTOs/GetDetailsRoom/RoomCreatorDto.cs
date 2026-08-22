namespace RafeekRoomMate.Application.Features.Rooms.DTOs.GetDetailsRoom
{
    public sealed class RoomCreatorDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Bio { get; init; } = string.Empty;

        public string? ProfileImageKey { get; init; }
    }
}
