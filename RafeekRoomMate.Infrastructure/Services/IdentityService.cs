using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Infrastructure.Persistence.Context;
using RafeekRoomMate.Infrastructure.Persistence.Identity;


namespace RafeekRoomMate.Infrastructure.Services
{
    public sealed class IdentityService : IIdentityService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public IdentityService(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> EmailExistsAsync( string email, CancellationToken cancellationToken = default)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<AuthResult> CreateUserAsync(string firstName, string lastName,
            string email, string password, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return AuthResult.Failure(result.Errors.Select(x => x.Description));
            }

            return AuthResult.Success(user.Id);
        }

        public async Task<Guid?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user?.Id;
        }

        public async Task<IReadOnlyCollection<string>> GetUserRolesAsync(Guid userId,CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Array.Empty<string>();

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToArray();
        }

        public async Task<AuthUserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return null;

            return new AuthUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty
            };
        }

        public async Task<bool> CheckPasswordAsync(Guid userId,string password,CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> AddToRoleAsync(Guid userId,string role, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return false;

            var result = await _userManager.AddToRoleAsync(user,role);

            return result.Succeeded;
        }

        public async Task<IReadOnlyCollection<AuthUserDto>> GetUsersAsync(IReadOnlyCollection<Guid> userIds,CancellationToken cancellationToken = default)
        {
            if (userIds.Count == 0)
            {
                return Array.Empty<AuthUserDto>();
            }

            var users = await _userManager.Users
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new AuthUserDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email!
                }).ToListAsync(cancellationToken);

            return users;
        }
        #endregion
    }
}
