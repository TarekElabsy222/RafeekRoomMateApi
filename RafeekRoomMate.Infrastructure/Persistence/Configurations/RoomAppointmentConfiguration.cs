using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RafeekRoomMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RafeekRoomMate.Infrastructure.Persistence.Configurations
{
    public sealed class RoomAppointmentConfiguration
        : IEntityTypeConfiguration<RoomAppointment>
    {
        public void Configure(
            EntityTypeBuilder<RoomAppointment> builder)
        {
            builder.ToTable("RoomAppointments");

            // Primary Key

            builder.HasKey(x => x.Id);

            // RoomId

            builder.Property(x => x.RoomId)
                .IsRequired();

            // UserId

            builder.Property(x => x.UserId)
                .IsRequired();

            // Date

            builder.Property(x => x.RequestedDate)
                .IsRequired();

            // Time

            builder.Property(x => x.RequestedStartTime)
                .IsRequired();

            builder.Property(x => x.RequestedEndTime)
                .IsRequired();

            // Message

            builder.Property(x => x.Message)
                .HasMaxLength(500);

            // Status

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            // Rejection Reason

            builder.Property(x => x.RejectionReason)
                .HasMaxLength(500);

            // Created At

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Relationship with Room

            builder.HasOne(x => x.Room) .WithMany(x => x.Appointments)
                     .HasForeignKey(x => x.RoomId)
                     .OnDelete(DeleteBehavior.Cascade);

            // Indexes

            builder.HasIndex(x => x.RoomId);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => new
            {
                x.RoomId,
                x.UserId,
                x.Status
            });
        }
    }
}
