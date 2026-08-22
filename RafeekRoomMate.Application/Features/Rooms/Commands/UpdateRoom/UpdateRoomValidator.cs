using FluentValidation;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.UpdateRoom
{
    public sealed class UpdateRoomValidator : AbstractValidator<UpdateRoomCommand>
    {
        public UpdateRoomValidator()
        {
           

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Room title is required.")
                .MaximumLength(150)
                .WithMessage("Room title cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Room description is required.")
                .MaximumLength(2000)
                .WithMessage("Room description cannot exceed 2000 characters.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(100)
                .WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Area)
                .NotEmpty()
                .WithMessage("Area is required.")
                .MaximumLength(100)
                .WithMessage("Area cannot exceed 100 characters.");

            RuleFor(x => x.MonthlyRent)
                .GreaterThan(0)
                .WithMessage("Monthly rent must be greater than zero.");

            RuleFor(x => x.AvailableFrom)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .When(x => x.AvailableFrom.HasValue)
                .WithMessage("Available from date cannot be in the past.");

            RuleFor(x => x.MaximumOccupants)
                .GreaterThan(0)
                .WithMessage("Maximum occupants must be greater than zero.");

            RuleFor(x => x.MaximumOccupants)
                .LessThanOrEqualTo(20)
                .WithMessage("Maximum occupants cannot exceed 20.");
        }
    }
}
