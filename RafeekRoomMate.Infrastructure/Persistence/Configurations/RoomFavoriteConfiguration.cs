using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Infrastructure.Persistence.Configurations
{
    public sealed class RoomFavoriteConfiguration : IEntityTypeConfiguration<RoomFavorite>
    {
        public void Configure( EntityTypeBuilder<RoomFavorite> builder)
        {
            // Table
            builder.ToTable("RoomFavorites");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Id
            builder.Property(x => x.Id) .ValueGeneratedNever();

            // RoomId
            builder.Property(x => x.RoomId) .IsRequired();

            // UserId
            builder.Property(x => x.UserId).IsRequired();

            // CreatedAt
            builder.Property(x => x.CreatedAt).IsRequired();

            // Room Relationship
            builder.HasOne(x => x.Room)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate favorite
            builder.HasIndex(x => new
            {
                x.UserId,
                x.RoomId
            })
            .IsUnique();
        }
    }

}
