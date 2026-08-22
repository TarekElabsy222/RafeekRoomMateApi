namespace RafeekRoomMate.Domain.Entities
{
    public sealed class RoomMember
    {
        private RoomMember(){ }

        public RoomMember(Guid roomId,Guid userId)
        {
            Id = Guid.NewGuid();

            RoomId = roomId;
            UserId = userId;

            JoinedAt = DateTime.UtcNow;

            IsActive = true;
        }

        public Guid Id { get; private set; }

        public Guid RoomId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTime JoinedAt { get; private set; }

        public DateTime? LeftAt { get; private set; }

        public bool IsActive { get; private set; }

        public Room Room { get; private set; } = null!;

        public void Leave()
        {
            if (!IsActive) return;

            IsActive = false;

            LeftAt = DateTime.UtcNow;
        }

        public void Rejoin()
        {
            IsActive = true;

            LeftAt = null;
        }

    }
}

