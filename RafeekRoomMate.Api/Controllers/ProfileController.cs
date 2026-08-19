using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafeekRoomMate.Application.Features.Profiles.Commands.CreateProfile;
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

        #endregion

        #region constructor
        public ProfileController(CreateProfileCommandHandler handler, GetMyProfileQueryHandler getMyProfileHandler, UpdateProfileCommandHandler updateProfileHandler, UploadProfileImageCommandHandler uploadProfileImageHandler)
        {
            _handler = handler;
            _getMyProfileHandler = getMyProfileHandler;
            _updateProfileHandler = updateProfileHandler;
            _uploadProfileImageHandler = uploadProfileImageHandler;
        }
        #endregion

        #region handle functions
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
        #endregion
    }
}
