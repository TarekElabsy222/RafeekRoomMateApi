using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Application.Features.Profiles.DTOs
{
    public sealed class UserProfileDto
    {
        public Guid Id { get; init; }

        public Guid UserId { get; init; }

        // Profile information
        public string Bio { get; init; } = string.Empty;

        public DateTime? DateOfBirth { get; init; }

        public Gender Gender { get; init; }

        public string Occupation { get; init; } = string.Empty;

        // Location
        public string City { get; init; } = string.Empty;

        public string Area { get; init; } = string.Empty;

        // Roommate preferences
        public decimal? MonthlyBudget { get; init; }

        public DateTime? MoveInDate { get; init; }

        public bool IsSmoking { get; init; }

        public bool HasPets { get; init; }

        public bool? LikesGuests { get; init; }

        public Gender PreferredRoommateGender { get; init; } 

        public int? PreferredAgeMin { get; init; }

        public int? PreferredAgeMax { get; init; }

        // Profile image
        public string? ProfileImageUrl { get; init; }

        // Audit
        public DateTime CreatedAt { get; init; }

        public DateTime? UpdatedAt { get; init; }
    }
}
