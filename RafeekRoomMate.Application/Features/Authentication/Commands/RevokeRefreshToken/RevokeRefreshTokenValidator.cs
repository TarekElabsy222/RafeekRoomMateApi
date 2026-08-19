using FluentValidation;

namespace RafeekRoomMate.Application.Features.Authentication.Commands.RevokeRefreshToken
{
    public sealed class RevokeRefreshTokenValidator : AbstractValidator<RevokeRefreshTokenCommand>
    {
        public RevokeRefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.")
                .MaximumLength(500)
                .WithMessage("Refresh token cannot exceed 500 characters.");
        }
    }
}
