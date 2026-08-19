using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Infrastructure.Persistence.Identity;

namespace RafeekRoomMate.Infrastructure.Persistence.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, UserRole, Guid>(options)
    {
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
