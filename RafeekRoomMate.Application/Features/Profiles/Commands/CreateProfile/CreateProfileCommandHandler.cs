using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Profiles.DTOs;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Profiles.Commands.CreateProfile
{
    public class CreateProfileCommandHandler
    {
        #region fields
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region constructor
        public CreateProfileCommandHandler(IRepository<UserProfile> userProfileRepository,IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        #endregion

        #region handle functions
        public async Task<Result<UserProfileDto>> HandleAsync(CreateProfileCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Get authenticated user ID
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<UserProfileDto>.Failure("AUTH_UNAUTHORIZED","User is not authenticated.");
            }
            // 2. Check if profile already exists
            var specification =
                new UserProfileByUserIdSpecification(userId.Value);

            var existingProfile = await _userProfileRepository.GetBySpecificationAsync(specification,cancellationToken);

            if (existingProfile is not null)
            {
                return Result<UserProfileDto>.Failure("PROFILE_ALREADY_EXISTS","User profile already exists.");
            }
            // 3. Create profile
            var profile = new UserProfile(userId.Value);

            // 4. Fill profile information
            profile.Update(command.Bio,command.DateOfBirth,command.Gender, command.Occupation,command.City,
                command.Area, command.MonthlyBudget, command.MoveInDate, command.IsSmoking,command.LikesGuests,
                command.HasPets, command.PreferredRoommateGender, command.PreferredAgeMin,command.PreferredAgeMax);

            // 5. Add to repository
            await _userProfileRepository.AddAsync(profile, cancellationToken);

            // 6. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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

                PreferredRoommateGender = profile.PreferredRoommateGender,

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
