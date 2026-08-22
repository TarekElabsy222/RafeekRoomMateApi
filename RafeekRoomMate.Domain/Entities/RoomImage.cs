namespace RafeekRoomMate.Domain.Entities
{
    public sealed class RoomImage
    {
        private RoomImage(){ }

        public RoomImage( Guid roomId, string imageKey, int displayOrder)
        {
            Id = Guid.NewGuid();

            RoomId = roomId;

            ImageKey = imageKey;

            DisplayOrder = displayOrder;

            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public Guid RoomId { get; private set; }

        public string ImageKey { get; private set; } = string.Empty;

        public int DisplayOrder { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Room Room { get; private set; } = null!;
    }
}
