using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Application.Features.Profiles.Commands.CreateProfile
{ 

    public sealed class CreateProfileCommand
    {
        public string Bio { get; init; } = string.Empty;

        public DateTime? DateOfBirth { get; init; }

        public Gender Gender { get; init; }

        public string Occupation { get; init; } = string.Empty;

        public string City { get; init; } = string.Empty;

        public string Area { get; init; } = string.Empty;

        public decimal? MonthlyBudget { get; init; }

        public DateTime? MoveInDate { get; init; }

        public bool IsSmoking { get; init; }

        public bool? LikesGuests { get; init; }

        public bool HasPets { get; init; }

        public Gender PreferredRoommateGender { get; init; }

        public int? PreferredAgeMin { get; init; }

        public int? PreferredAgeMax { get; init; }
    }
}
