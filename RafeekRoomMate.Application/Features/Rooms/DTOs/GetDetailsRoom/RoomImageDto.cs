namespace RafeekRoomMate.Application.Features.Rooms.DTOs.GetDetailsRoom
{
    public sealed class RoomImageDto
    {
        public Guid Id { get; init; }

        public string ImageKey { get; init; } = string.Empty;

        public int DisplayOrder { get; init; }
    }
}
