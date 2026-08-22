namespace RafeekRoomMate.Application.Features.Rooms.Commands.AddRoomImage
{
    public sealed class AddRoomImageCommand
    {
        public Guid RoomId { get; init; }

        public Stream Stream { get; init; } = default!;

        public string FileName { get; init; } = string.Empty;

        public string ContentType { get; init; } = string.Empty;

        public long Length { get; init; }

    }
}
