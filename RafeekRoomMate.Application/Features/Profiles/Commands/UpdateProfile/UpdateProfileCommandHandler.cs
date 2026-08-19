using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Profiles.DTOs;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Profiles.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler
    {
        #region fields
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public UpdateProfileCommandHandler(IRepository<UserProfile> userProfileRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<Result<UserProfileDto>> HandleAsync(UpdateProfileCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Get current authenticated user
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<UserProfileDto>.Failure("AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Find current user's profile
            var specification = new UserProfileByUserIdSpecification(userId.Value);

            var profile = await _userProfileRepository.GetBySpecificationAsync( specification, cancellationToken);

            // 3. Profile must already exist
            if (profile is null)
            {
                return Result<UserProfileDto>.Failure( "PROFILE_NOT_FOUND", "User profile has not been completed.");
            }
            // 4. Update entity
            profile.Update(
                command.Bio,command.DateOfBirth,
                command.Gender, command.Occupation,
                command.City,command.Area,
                command.MonthlyBudget,  command.MoveInDate,
                command.IsSmoking, command.LikesGuests,
                command.HasPets, command.PreferredRoommateGender,
                command.PreferredAgeMin,command.PreferredAgeMax);
            // 5. Save changes
            await _unitOfWork.SaveChangesAsync( cancellationToken);

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
