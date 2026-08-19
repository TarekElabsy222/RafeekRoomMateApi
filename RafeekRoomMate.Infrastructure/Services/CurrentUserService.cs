using Microsoft.AspNetCore.Http;
using RafeekRoomMate.Application.Common.Interfaces;
using System.Security.Claims;

namespace RafeekRoomMate.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        #region fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion


        #region constructor
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region handle functions
        public Guid? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(userId, out var id) ? id : null;
            }
        }
        #endregion
    }
}
