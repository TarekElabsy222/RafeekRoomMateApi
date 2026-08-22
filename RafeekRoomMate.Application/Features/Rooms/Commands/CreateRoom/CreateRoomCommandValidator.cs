using FluentValidation;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.CreateRoom
{
    public sealed class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);

            RuleFor(x => x.City).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Area).NotEmpty().MaximumLength(150);

            RuleFor(x => x.MonthlyRent).GreaterThan(0);

            RuleFor(x => x.MaximumOccupants).InclusiveBetween(1, 20);

            RuleFor(x => x.AvailableFrom).GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .When(x => x.AvailableFrom.HasValue);

            RuleFor(x => x.RoomType).IsInEnum();

            RuleFor(x => x.GenderPreference).IsInEnum();
        }
    }
}
