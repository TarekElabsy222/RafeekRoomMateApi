using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Infrastructure.Persistence.Identity;

namespace RafeekRoomMate.Infrastructure.Persistence.Configurations
{
    public sealed class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Token).HasMaxLength(500).IsRequired();

            builder.HasIndex(x => x.Token).IsUnique();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.ExpiresAt).IsRequired();

            builder.Property(x => x.ReplacedByToken).HasMaxLength(500);

            builder.Property(x => x.UserId).IsRequired();

            builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
