using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Profiles.DTOs;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Profiles.Queries.GetMyProfile
{
    public class GetMyProfileQueryHandler
    {
        #region fields
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region constructor
        public GetMyProfileQueryHandler(
        IRepository<UserProfile> userProfileRepository,
        ICurrentUserService currentUserService)
        {
            _userProfileRepository = userProfileRepository;
            _currentUserService = currentUserService;
        }
        #endregion

        #region handle functions
        public async Task<Result<UserProfileDto>> HandleAsync(GetMyProfileQuery query, CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<UserProfileDto>.Failure("AUTH_UNAUTHORIZED","User is not authenticated.");
            }
            var specification = new UserProfileByUserIdSpecification(userId.Value);

            var profile = await _userProfileRepository.GetBySpecificationAsync(specification, cancellationToken);

            if (profile is null)
            {
                return Result<UserProfileDto>.Failure("PROFILE_NOT_FOUND",  "User profile has not been completed.");
            }
            var response = new UserProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,

                Bio = profile.Bio,
                DateOfBirth = profile.DateOfBirth,
                Gender = profile.Gender,
                Occupation = profile.Occupation,

                City = profile.City,
                Area = profile.Area,

                MonthlyBudget = profile.MonthlyBudget,
                MoveInDate = profile.MoveInDate,

                IsSmoking = profile.IsSmoking,
                HasPets = profile.HasPets,
                LikesGuests = profile.LikesGuests,

                PreferredRoommateGender =
                            profile.PreferredRoommateGender,

                PreferredAgeMin = profile.PreferredAgeMin,
                PreferredAgeMax = profile.PreferredAgeMax,

                ProfileImageUrl = null,

                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt
            };

            return Result<UserProfileDto>.Success(response);
        }
        #endregion
    }
}
