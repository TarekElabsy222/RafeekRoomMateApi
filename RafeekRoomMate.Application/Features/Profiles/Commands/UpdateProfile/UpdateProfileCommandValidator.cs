using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RafeekRoomMate.Application.Features.Profiles.Commands.UpdateProfile
{


    public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.Bio).MaximumLength(1000);

            RuleFor(x => x.Occupation).NotEmpty().MaximumLength(150);

            RuleFor(x => x.City).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Area).NotEmpty().MaximumLength(150);

            RuleFor(x => x.MonthlyBudget).GreaterThan(0).When(x => x.MonthlyBudget.HasValue);

            RuleFor(x => x.PreferredAgeMin).GreaterThanOrEqualTo(18).When(x => x.PreferredAgeMin.HasValue);

            RuleFor(x => x.PreferredAgeMax).GreaterThanOrEqualTo(18).When(x => x.PreferredAgeMax.HasValue);

            RuleFor(x => x)
                .Must(x =>!x.PreferredAgeMin.HasValue || !x.PreferredAgeMax.HasValue || x.PreferredAgeMin <= x.PreferredAgeMax)
                .WithMessage(
                    "Preferred minimum age must be less than or equal to maximum age.");

            RuleFor(x => x.MoveInDate).GreaterThanOrEqualTo(DateTime.UtcNow.Date).When(x => x.MoveInDate.HasValue);
        }
    }
}
