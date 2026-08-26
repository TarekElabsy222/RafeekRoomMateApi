using RafeekRoomMate.Application.Common.Models;

namespace RafeekRoomMate.Application.Common.Interfaces
{
    public interface IRoomAppointmentReader
    {
        Task<IReadOnlyList<RoomAppointmentReadModel>> GetByRoomAsync( Guid roomId, int skip, int take, CancellationToken cancellationToken = default);

        Task<int> CountByRoomAsync( Guid roomId, CancellationToken cancellationToken = default);
    }
}
