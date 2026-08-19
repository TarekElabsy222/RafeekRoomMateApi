using RafeekRoomMate.Application.Features.Authentication.Common;

namespace RafeekRoomMate.Application.Features.Authentication.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler
    {
        #region fields
        private readonly IIdentityService _identityService;
        #endregion

        #region constructor
        public GetCurrentUserQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        #endregion

        #region handle functions
        public async Task<AuthUserDto?> HandleAsync(Guid userId,  CancellationToken cancellationToken = default)
        {
            return await _identityService.GetUserAsync(userId,cancellationToken);
        }
        #endregion
    }
}
