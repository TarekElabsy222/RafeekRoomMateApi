using FluentValidation;

namespace RafeekRoomMate.Application.Features.RoomAppointments.Commands.RejectRoomAppointment
{
    public sealed class RejectRoomAppointmentCommandValidator : AbstractValidator<RejectRoomAppointmentCommand>
    {
        public RejectRoomAppointmentCommandValidator()
        {
            RuleFor(x => x.RoomId)
                .NotEmpty()
                .WithMessage("Room ID is required.");

            RuleFor(x => x.AppointmentId)
                .NotEmpty()
                .WithMessage("Appointment ID is required.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Rejection reason is required.")
                .MaximumLength(500)
                .WithMessage("Rejection reason cannot exceed 500 characters.");
        }
    }
}
