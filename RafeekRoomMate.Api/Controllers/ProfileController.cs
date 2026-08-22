using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafeekRoomMate.Application.Features.Profiles.Commands.CreateProfile;
using RafeekRoomMate.Application.Features.Profiles.Commands.DeleteProfileImage;
using RafeekRoomMate.Application.Features.Profiles.Commands.UpdateProfile;
using RafeekRoomMate.Application.Features.Profiles.Commands.UploadProfileImage;
using RafeekRoomMate.Application.Features.Profiles.Queries.GetMyProfile;

namespace RafeekRoomMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        #region fields
        private readonly CreateProfileCommandHandler _handler;
        private readonly GetMyProfileQueryHandler _getMyProfileHandler;
        private readonly UpdateProfileCommandHandler _updateProfileHandler;
        private readonly UploadProfileImageCommandHandler _uploadProfileImageHandler; 
        private readonly DeleteProfileImageCommandHandler _deleteProfileImageHandler;

        #endregion

        #region constructor
        public ProfileController(CreateProfileCommandHandler handler, GetMyProfileQueryHandler getMyProfileHandler, UpdateProfileCommandHandler updateProfileHandler, UploadProfileImageCommandHandler uploadProfileImageHandler, DeleteProfileImageCommandHandler deleteProfileImageHandler)
        {
            _handler = handler;
            _getMyProfileHandler = getMyProfileHandler;
            _updateProfileHandler = updateProfileHandler;
            _uploadProfileImageHandler = uploadProfileImageHandler;
            _deleteProfileImageHandler = deleteProfileImageHandler;
        }
        #endregion

        #region handle functions
        /// <summary>
        /// Creates a new user profile.
        /// </summary>
        /// <remarks>
        /// Creates a profile for the authenticated user using the provided profile information.
        /// </remarks>
        /// <response code="200">Profile created successfully.</response>
        /// <response code="400">The profile data is invalid or the profile could not be created.</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProfileCommand command, CancellationToken cancellationToken)
        {
            var result = await _handler.HandleAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Retrieves the profile of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// Requires a valid JWT access token.
        /// The user identity is obtained from the authenticated request.
        /// </remarks>
        /// <response code="200">The user's profile was retrieved successfully.</response>
        /// <response code="400">The profile could not be retrieved.</response>
        /// <response code="401">The user is not authenticated.</response>
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var result = await _getMyProfileHandler.HandleAsync(new GetMyProfileQuery(), cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Updates the profile of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// Requires a valid JWT access token.
        /// The profile information is provided in the request body.
        /// </remarks>
        /// <response code="200">Profile updated successfully.</response>
        /// <response code="400">The profile data is invalid or the update failed.</response>
        /// <response code="401">The user is not authenticated.</response>
        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileCommand command,CancellationToken cancellationToken)
        {
            var result = await _updateProfileHandler.HandleAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Uploads a profile image for the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// Requires a valid JWT access token.
        /// The profile image must be sent as a multipart/form-data request.
        /// </remarks>
        /// <response code="200">Profile image uploaded successfully.</response>
        /// <response code="400">The profile image is missing, empty, invalid, or the upload failed.</response>
        /// <response code="401">The user is not authenticated.</response>
        [Authorize]
        [HttpPost("me/image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file, CancellationToken cancellationToken)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("Profile image is required.");
            }

            await using var stream = file.OpenReadStream();

            var command = new UploadProfileImageCommand
            {
                Stream = stream,
                FileName = file.FileName,
                ContentType = file.ContentType,
                Length = file.Length
            };

            var result =await _uploadProfileImageHandler.HandleAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes the profile image of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// Requires a valid JWT access token.
        /// Removes the currently stored profile image from the user's profile.
        /// </remarks>
        /// <response code="200">Profile image deleted successfully.</response>
        /// <response code="400">The profile image could not be deleted.</response>
        /// <response code="401">The user is not authenticated.</response>
        [Authorize]
        [HttpDelete("me/image")]
        public async Task<IActionResult> DeleteProfileImage( CancellationToken cancellationToken)
        {
            var result = await _deleteProfileImageHandler.HandleAsync( new DeleteProfileImageCommand(),  cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        #endregion
    }
}
