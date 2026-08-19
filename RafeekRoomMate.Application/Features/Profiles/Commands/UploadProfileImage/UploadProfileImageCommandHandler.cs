using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Profiles.DTOs;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Profiles.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandHandler
    {
        #region fields
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public UploadProfileImageCommandHandler(IRepository<UserProfile> userProfileRepository, ICurrentUserService currentUserService, IFileStorageService fileStorageService, IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions

        public async Task<Result<UserProfileDto>> HandleAsync(UploadProfileImageCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Get current user
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<UserProfileDto>.Failure("AUTH_UNAUTHORIZED","User is not authenticated.");
            }
            // 2. Get profile
            var specification = new UserProfileByUserIdSpecification(userId.Value);

            var profile = await _userProfileRepository.GetBySpecificationAsync( specification, cancellationToken);

            if (profile is null)
            {
                return Result<UserProfileDto>.Failure( "PROFILE_NOT_FOUND", "User profile has not been completed.");
            }
            // 3. Upload new image
            var imageKey = await _fileStorageService.UploadAsync(command.Stream, command.FileName, command.ContentType,
                $"profiles/{userId.Value}",cancellationToken);

            // 4. Keep old key
            var oldImageKey = profile.ProfileImageKey;

            // 5. Update profile
            profile.UpdateProfileImage(imageKey);

            // 6. Save database changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Delete old image AFTER database succeeds
            if (!string.IsNullOrWhiteSpace(oldImageKey))
            {
                await _fileStorageService.DeleteAsync( oldImageKey, cancellationToken);
            }

            // 8. Return DTO
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

                ProfileImageUrl = imageKey,

                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt
            };

            return Result<UserProfileDto>.Success(response);
        }
        #endregion
        }
}
