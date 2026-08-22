namespace RafeekRoomMate.Domain.Entities
{
    public sealed class RoomFavorite
    {
        private RoomFavorite()
        {
        }

        public RoomFavorite( Guid roomId, Guid userId)
        {
            Id = Guid.NewGuid();

            RoomId = roomId;

            UserId = userId;

            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public Guid RoomId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Room Room { get; private set; } = null!;
    }
}
