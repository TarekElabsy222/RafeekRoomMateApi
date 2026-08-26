using FluentValidation;

namespace RafeekRoomMate.Application.Features.RoomAppointments.Commands.CreateRoomAppointment
{
    public sealed class CreateRoomAppointmentCommandValidator : AbstractValidator<CreateRoomAppointmentCommand>
    {
        public CreateRoomAppointmentCommandValidator()
        {

            RuleFor(x => x.RequestedDate)
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .WithMessage("Appointment date cannot be in the past.");

            RuleFor(x => x.RequestedStartTime)
                .LessThan(x => x.RequestedEndTime)
                .WithMessage("Start time must be before end time.");

            RuleFor(x => x.RequestedEndTime)
                .GreaterThan(x => x.RequestedStartTime)
                .WithMessage("End time must be after start time.");

            RuleFor(x => x.Message)
                .MaximumLength(500)
                .WithMessage("Message cannot exceed 500 characters.");
        }
    }
}
