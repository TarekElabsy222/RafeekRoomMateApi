using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Infrastructure.Persistence.Configurations
{
    public sealed class RoomImageConfiguration: IEntityTypeConfiguration<RoomImage>
    {
        public void Configure( EntityTypeBuilder<RoomImage> builder)
        {
            builder.ToTable("RoomImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageKey).HasMaxLength(500).IsRequired();

            builder.Property(x => x.DisplayOrder).IsRequired();

            builder.HasIndex(x => x.RoomId);

            builder.HasIndex(x => new
            {
                x.RoomId,
                x.DisplayOrder
            });

            builder.HasOne(x => x.Room).WithMany(x => x.Images).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
