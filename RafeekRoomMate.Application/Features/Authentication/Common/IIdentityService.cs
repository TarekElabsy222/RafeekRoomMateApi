using RafeekRoomMate.Application.Common.Models;

namespace RafeekRoomMate.Application.Features.Authentication.Common
{
    public interface IIdentityService
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        Task<AuthResult> CreateUserAsync(string firstName, string lastName,
            string email, string password,CancellationToken cancellationToken = default);

        Task<Guid?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<string>> GetUserRolesAsync( Guid userId, CancellationToken cancellationToken = default);

        Task<AuthUserDto?> GetUserAsync( Guid userId, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<AuthUserDto>> GetUsersAsync( IReadOnlyCollection<Guid> userIds,CancellationToken cancellationToken = default);

        Task<bool> CheckPasswordAsync( Guid userId, string password, CancellationToken cancellationToken = default);

        Task<bool> AddToRoleAsync( Guid userId, string role, CancellationToken cancellationToken = default);


    }
}
