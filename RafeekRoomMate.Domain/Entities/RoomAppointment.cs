using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Domain.Entities
{
    public sealed class RoomAppointment
    {
        private RoomAppointment()
        {
        }

        public RoomAppointment( Guid roomId, Guid userId, DateTime requestedDate, TimeSpan requestedStartTime, TimeSpan requestedEndTime, string? message)
        {
            Id = Guid.NewGuid();

            RoomId = roomId;

            UserId = userId;

            RequestedDate = requestedDate;

            RequestedStartTime = requestedStartTime;

            RequestedEndTime = requestedEndTime;

            Message = message;

            Status = RoomAppointmentStatus.Pending;

            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        // Room

        public Guid RoomId { get; private set; }

        public Room Room { get; private set; } = null!;

        // Applicant

        public Guid UserId { get; private set; }

        // Appointment

        public DateTime RequestedDate { get; private set; }

        public TimeSpan RequestedStartTime { get; private set; }

        public TimeSpan RequestedEndTime { get; private set; }

        public string? Message { get; private set; }

        // Status

        public RoomAppointmentStatus Status { get; private set; }

        // Rejection

        public string? RejectionReason { get; private set; }

        // Audit

        public DateTime CreatedAt { get; private set; }

        public DateTime? RespondedAt { get; private set; }

        public DateTime? CancelledAt { get; private set; }

        // Domain behavior

        public void Approve()
        {
            if (Status != RoomAppointmentStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending appointments can be approved.");
            }

            Status = RoomAppointmentStatus.Approved;

            RespondedAt = DateTime.UtcNow;
        }

        public void Reject(string? reason)
        {
            if (Status != RoomAppointmentStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending appointments can be rejected.");
            }

            Status = RoomAppointmentStatus.Rejected;

            RejectionReason = reason;

            RespondedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status != RoomAppointmentStatus.Pending)
                return;

            Status = RoomAppointmentStatus.Cancelled;

            CancelledAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status != RoomAppointmentStatus.Approved)
                return;

            Status = RoomAppointmentStatus.Completed;
        }
    }
}
