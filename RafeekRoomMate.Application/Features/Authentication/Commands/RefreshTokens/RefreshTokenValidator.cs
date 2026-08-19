using FluentValidation;

namespace RafeekRoomMate.Application.Features.Authentication.Commands.RefreshTokens
{
    public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.")
                .MaximumLength(500)
                .WithMessage("Refresh token cannot exceed 500 characters.");
        }
    }
}
