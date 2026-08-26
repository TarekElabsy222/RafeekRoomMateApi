namespace RafeekRoomMate.Application.Features.RoomAppointments.Dto
{
    public sealed class RoomAppointmentDto
    {
        public Guid Id { get; init; }

        public Guid UserId { get; init; }

        public string UserName { get; init; } = string.Empty;

        public string? UserBio { get; init; }

        public string? UserImageKey { get; init; }

        public DateTime RequestedDate { get; init; }

        public TimeSpan RequestedStartTime { get; init; }

        public TimeSpan RequestedEndTime { get; init; }

        public string? Message { get; init; }

        public string Status { get; init; } = string.Empty;

        public string? RejectionReason { get; init; }

        public DateTime CreatedAt { get; init; }

        public DateTime? RespondedAt { get; init; }
    }

}
