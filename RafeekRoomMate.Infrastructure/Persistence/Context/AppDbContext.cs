using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Infrastructure.Persistence.Identity;

namespace RafeekRoomMate.Infrastructure.Persistence.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, UserRole, Guid>(options)
    {
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RoomImage> RoomImages => Set<RoomImage>();
        public DbSet<RoomMember> RoomMembers => Set<RoomMember>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<RoomFavorite> RoomFavorites => Set<RoomFavorite>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
