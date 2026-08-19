using FluentValidation;

namespace RafeekRoomMate.Application.Features.Authentication.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);

            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
