using Microsoft.AspNetCore.Identity;

namespace RafeekRoomMate.Infrastructure.Persistence.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
