using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Infrastructure.Persistence.Configurations
{
    public sealed class RoomMemberConfiguration : IEntityTypeConfiguration<RoomMember>
    {
        public void Configure(EntityTypeBuilder<RoomMember> builder)
        {
            builder.ToTable("RoomMembers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.IsActive).IsRequired();

            builder.HasIndex(x => x.RoomId);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => new
            {
                x.RoomId,
                x.UserId
            })
            .IsUnique();

            builder.HasOne(x => x.Room).WithMany(x => x.Members).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
