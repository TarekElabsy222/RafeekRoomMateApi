using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Profiles.Commands.DeleteProfileImage
{
    public sealed class DeleteProfileImageCommandHandler
    {
        #region fields
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public DeleteProfileImageCommandHandler( IRepository<UserProfile> userProfileRepository, ICurrentUserService currentUserService, IFileStorageService fileStorageService,IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region handle functions
        public async Task<Result<bool>> HandleAsync( DeleteProfileImageCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Get current user
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<bool>.Failure("AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Get profile
            var specification =  new UserProfileByUserIdSpecification(userId.Value);

            var profile = await _userProfileRepository.GetBySpecificationAsync( specification, cancellationToken);

            if (profile is null)
            {
                return Result<bool>.Failure("PROFILE_NOT_FOUND", "User profile has not been completed.");
            }

            // 3. Check image
            var imageKey = profile.ProfileImageKey;

            if (string.IsNullOrWhiteSpace(imageKey))
            {
                return Result<bool>.Failure("PROFILE_IMAGE_NOT_FOUND","Profile image does not exist.");
            }

            // 4. Remove image reference from entity
            profile.RemoveProfileImage();

            // 5. Save DB first
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. Delete physical file
            await _fileStorageService.DeleteAsync( imageKey,cancellationToken);

            return Result<bool>.Success(true);
        }
    }
        #endregion
}

