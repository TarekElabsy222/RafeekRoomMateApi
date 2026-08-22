using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <remarks>
        /// Creates a new user account using the provided registration information
        /// and returns the authentication result.
        /// </remarks>        
        /// <returns>
        /// The authentication result containing the registration status and user information.
        /// </returns>
        /// <response code="200">User registered successfully.</response>
        /// <response code="401">Registration failed.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command,CancellationToken cancellationToken)
        {
            var result = await _registerHandler.HandleAsync(command,cancellationToken);
            
            if (!result.IsSuccess) return Unauthorized(result);            

            return Ok(result);
        }
        /// <summary>
        /// Authenticates an existing user.
        /// </summary>
        /// <remarks>
        /// Validates the user's credentials and returns authentication information
        /// including the access token and refresh token.
        /// </remarks>        
        /// <returns>
        /// The authentication result containing access and refresh tokens.
        /// </returns>
        /// <response code="200">Login successful.</response>
        /// <response code="401">Invalid credentials.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command,CancellationToken cancellationToken)
        {
            var result = await _loginHandler.HandleAsync(command, cancellationToken);
            
            if (!result.IsSuccess) return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>
        /// Generates a new access token using a refresh token.
        /// </summary>
        /// <remarks>
        /// Validates the provided refresh token and generates a new access token
        /// when the refresh token is valid and has not expired or been revoked.
        /// </remarks>
        /// <returns>
        /// A new authentication response containing the refreshed tokens.
        /// </returns>
        /// <response code="200">Access token refreshed successfully.</response>
        /// <response code="400">The refresh token is invalid or expired.</response>

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var response = await _refreshTokenHandler.HandleAsync(command, cancellationToken);

            return Ok(response);
        }
        /// <summary>
        /// Revokes the current refresh token and logs the user out.
        /// </summary>
        /// <remarks>
        /// Invalidates the specified refresh token so it can no longer be used
        /// to obtain a new access token.
        /// </remarks>        
        /// Token used to cancel the request.
        /// <returns>
        /// A confirmation message when the user is successfully logged out.
        /// </returns>
        /// <response code="200">User logged out successfully.</response>
        /// <response code="404">The specified refresh token was not found.</response>
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

        /// <summary>
        /// Retrieves the currently authenticated user's information.
        /// </summary>
        /// <remarks>
        /// Extracts the authenticated user's ID from the JWT claims and retrieves
        /// the corresponding user information.
        /// </remarks>
        /// <returns>
        /// The profile information of the currently authenticated user.
        /// </returns>
        /// <response code="200">Current user information retrieved successfully.</response>
        /// <response code="401">The user is not authenticated or the user ID is invalid.</response>
        /// <response code="404">The authenticated user was not found.</response>
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
