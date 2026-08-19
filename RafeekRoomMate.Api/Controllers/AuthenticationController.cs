using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Authentication.Commands.Login;
using RafeekRoomMate.Application.Features.Authentication.Commands.RefreshTokens;
using RafeekRoomMate.Application.Features.Authentication.Commands.RevokeRefreshToken;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Application.Features.Authentication.Queries.GetCurrentUser;
using RafeekRoomMate.Application.Features.Authentication.Register;
using System.Security.Claims;

namespace RafeekRoomMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region fields
        private readonly RegisterCommandHandler _registerHandler;
        private readonly LoginCommandHandler _loginHandler;
        private readonly RefreshTokenCommandHandler _refreshTokenHandler;
        private readonly RevokeRefreshTokenCommandHandler _revokeHandler;
        private readonly GetCurrentUserQueryHandler _currentUserHandler;
        #endregion

        #region constructor
        public AuthenticationController(RegisterCommandHandler registerHandler, LoginCommandHandler loginHandler, RefreshTokenCommandHandler refreshTokenHandler, RevokeRefreshTokenCommandHandler revokeHandler, GetCurrentUserQueryHandler currentUserHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
            _refreshTokenHandler = refreshTokenHandler;
            _revokeHandler = revokeHandler;
            _currentUserHandler = currentUserHandler;
        }
        #endregion

        #region handle functions
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command,CancellationToken cancellationToken)
        {
            var result = await _registerHandler.HandleAsync(command,cancellationToken);
            
            if (!result.IsSuccess) return Unauthorized(result);            

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command,CancellationToken cancellationToken)
        {
            var result = await _loginHandler.HandleAsync(command, cancellationToken);
            
            if (!result.IsSuccess) return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var response = await _refreshTokenHandler.HandleAsync(command, cancellationToken);

            return Ok(response);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RevokeRefreshTokenCommand command,CancellationToken cancellationToken)
        {
            var result = await _revokeHandler.HandleAsync(command,   cancellationToken);

            if (!result)
            {
                return NotFound("Refresh token not found.");
            }

            return Ok(new
            {
                message = "Logged out successfully."
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<AuthUserDto>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var user = await _currentUserHandler.HandleAsync(userId,cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        #endregion

    }
}
