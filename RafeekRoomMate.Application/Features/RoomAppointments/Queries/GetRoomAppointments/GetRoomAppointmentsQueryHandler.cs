using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.RoomAppointments.Dto;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.Rooms;

namespace RafeekRoomMate.Application.Features.RoomAppointments.Queries.GetRoomAppointments
{
    public sealed class GetRoomAppointmentsQueryHandler
    {
        #region fields

        private readonly IRepository<Room> _roomRepository;
        private readonly IRoomAppointmentReader _appointmentReader;
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region constructor

        public GetRoomAppointmentsQueryHandler(
            IRepository<Room> roomRepository,
            IRoomAppointmentReader appointmentReader,
            ICurrentUserService currentUserService)
        {
            _roomRepository = roomRepository;
            _appointmentReader = appointmentReader;
            _currentUserService = currentUserService;
        }

        #endregion

        #region handle functions

        public async Task<Result<PagedResult<RoomAppointmentDto>>> HandleAsync(
            GetRoomAppointmentsQuery query,
            CancellationToken cancellationToken = default)
        {
            // 1. Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<PagedResult<RoomAppointmentDto>>.Failure(
                    "AUTH_UNAUTHORIZED",
                    "User is not authenticated.");
            }

            // 2. Pagination

            var pageNumber =
                query.PageNumber < 1
                    ? 1
                    : query.PageNumber;

            var pageSize =
                query.PageSize < 1
                    ? 10
                    : Math.Min(query.PageSize, 50);

            var skip =
                (pageNumber - 1) * pageSize;

            // 3. Get Room

            var roomSpecification =
                new RoomByIdSpecification(query.RoomId);

            var room =
                await _roomRepository.GetBySpecificationAsync(
                    roomSpecification,
                    cancellationToken);

            if (room is null)
            {
                return Result<PagedResult<RoomAppointmentDto>>.Failure(
                    "ROOM_NOT_FOUND",
                    "Room was not found.");
            }

            // 4. Only Owner can see requests

            if (room.CreatedByUserId != userId.Value)
            {
                return Result<PagedResult<RoomAppointmentDto>>.Failure(
                    "ROOM_ACCESS_DENIED",
                    "Only the room owner can view appointment requests.");
            }

            // 5. Get appointments

            var appointments =
                await _appointmentReader.GetByRoomAsync(
                    query.RoomId,
                    skip,
                    pageSize,
                    cancellationToken);

            // 6. Total count

            var totalCount =
                await _appointmentReader.CountByRoomAsync(
                    query.RoomId,
                    cancellationToken);

            // 7. Map

            var items =
                appointments
                    .Select(x => new RoomAppointmentDto
                    {
                        Id = x.Id,

                        UserId = x.UserId,

                        UserName = x.UserName,

                        UserBio = x.UserBio,

                        UserImageKey = x.UserImageUrl,

                        RequestedDate = x.RequestedDate,

                        RequestedStartTime =
                            x.RequestedStartTime,

                        RequestedEndTime =
                            x.RequestedEndTime,

                        Message = x.Message,

                        Status = x.Status,

                        RejectionReason =
                            x.RejectionReason,

                        CreatedAt = x.CreatedAt,

                        RespondedAt = x.RespondedAt
                    })
                    .ToList();

            // 8. Paged Result

            var result = new PagedResult<RoomAppointmentDto>
            {
                Items = items,

                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalCount = totalCount
            };

            // 9. Return

            return Result<PagedResult<RoomAppointmentDto>>.Success(
                result);
        }

        #endregion
    }
}