using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Infrastructure.Persistence.Configurations
{
    public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();

            builder.Property(x => x.City).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Area).HasMaxLength(150).IsRequired();

            builder.Property(x => x.GenderPreference).IsRequired();

            builder.Property(x => x.RoomType).IsRequired();

            builder.Property(x => x.MonthlyRent).HasPrecision(18, 2).IsRequired();

            builder.Property(x => x.MaximumOccupants).IsRequired();

            builder.Property(x => x.Status).IsRequired();

            builder.HasIndex(x => x.CreatedByUserId);

            builder.HasIndex(x => x.City);

            builder.HasIndex(x => x.Area);

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.RoomType);

            builder.HasIndex(x => x.GenderPreference);

            builder.HasIndex(x => new
            {
                x.City,
                x.Area,
                x.Status
            });
        }
    }


}
