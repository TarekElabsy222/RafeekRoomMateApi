using Microsoft.AspNetCore.Identity;
using RafeekRoomMate.Domain.Constant;
using RafeekRoomMate.Infrastructure.Persistence.Identity;

namespace RafeekRoomMate.Infrastructure.Persistence.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            RoleManager<UserRole> roleManager)
        {
            const string userRole = Roles.User;

            var roleExists =  await roleManager.RoleExistsAsync(userRole);

            if (!roleExists)
            {
                var role = new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = userRole,
                    NormalizedName = userRole.ToUpperInvariant()
                };

                var result =  await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join( ", ", result.Errors.Select( x => x.Description)));
                }
            }
        }
    }
}
