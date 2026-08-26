using Microsoft.EntityFrameworkCore;
using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Infrastructure.Persistence.Context;

namespace RafeekRoomMate.Infrastructure.Services
{
    public sealed class RoomAppointmentReader : IRoomAppointmentReader
    {
        private readonly AppDbContext _context;

        public RoomAppointmentReader(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<RoomAppointmentReadModel>> GetByRoomAsync( Guid roomId, int skip, int take, CancellationToken cancellationToken = default)
        {
            var query = from appointment in _context.Appointments.AsNoTracking()

                join user in _context.Users.AsNoTracking()
                    on appointment.UserId equals user.Id

                join profile in _context.UserProfiles.AsNoTracking()
                    on appointment.UserId equals profile.UserId
                    into profiles

                from profile in profiles.DefaultIfEmpty()

                where appointment.RoomId == roomId

                orderby appointment.CreatedAt descending

                select new RoomAppointmentReadModel
                {
                    Id = appointment.Id,

                    UserId = appointment.UserId,

                    UserName = $"{user.FirstName} {user.LastName}".Trim(),

                    UserBio = profile != null ? profile.Bio : null,

                    UserImageUrl = profile != null ? profile.ProfileImageKey : null,

                    RequestedDate = appointment.RequestedDate,

                    RequestedStartTime = appointment.RequestedStartTime,

                    RequestedEndTime = appointment.RequestedEndTime,

                    Message = appointment.Message,

                    Status = appointment.Status.ToString(),

                    RejectionReason = appointment.RejectionReason,

                    CreatedAt = appointment.CreatedAt,

                    RespondedAt = appointment.RespondedAt
                };

            return await query.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public async Task<int> CountByRoomAsync( Guid roomId, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments .AsNoTracking() .CountAsync( x => x.RoomId == roomId, cancellationToken);
        }
    }
}
