using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Domain.Entities
{

    public sealed class UserProfile
    {
        private UserProfile()
        {
        }

        public UserProfile(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        // Profile information
        public string Bio { get; private set; } = string.Empty;

        public DateTime? DateOfBirth { get; private set; }

        public Gender Gender { get; private set; }

        public string Occupation { get; private set; } = string.Empty;

        // Location
        public string City { get; private set; } = string.Empty;

        public string Area { get; private set; } = string.Empty;

        // Roommate preferences
        public decimal? MonthlyBudget { get; private set; }

        public DateTime? MoveInDate { get; private set; }

        public bool IsSmoking { get; private set; }

        public bool HasPets { get; private set; }
        public bool? LikesGuests { get; private set; }

        public Gender PreferredRoommateGender { get; private set; } 

        public int? PreferredAgeMin { get; private set; }

        public int? PreferredAgeMax { get; private set; }

        // Profile image
        public string? ProfileImageKey { get; private set; }

        // Audit
        public DateTime CreatedAt { get; private set; }

        public DateTime? UpdatedAt { get; private set; }


        public void Update(string bio,DateTime? dateOfBirth, Gender gender, string occupation,string city,string area,
            decimal? monthlyBudget,DateTime? moveInDate, bool isSmoking, bool? likesGuests, bool hasPets, Gender preferredRoommateGender,
            int? preferredAgeMin, int? preferredAgeMax)
        {
            Bio = bio;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Occupation = occupation;
            City = city;
            Area = area;
            MonthlyBudget = monthlyBudget;
            MoveInDate = moveInDate;
            IsSmoking = isSmoking;
            LikesGuests = likesGuests;
            HasPets = hasPets;
            PreferredRoommateGender = preferredRoommateGender;
            PreferredAgeMin = preferredAgeMin;
            PreferredAgeMax = preferredAgeMax;

            UpdatedAt = DateTime.UtcNow;
        }


        public void UpdateProfileImage(string imageKey)
        {
            ProfileImageKey = imageKey;
            UpdatedAt = DateTime.UtcNow;
        }


        public void RemoveProfileImage()
        {
            ProfileImageKey = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
