using RafeekRoomMate.Application.Common.Interfaces;
using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.RoomFavorites.Dto;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.RoomFavorites;

namespace RafeekRoomMate.Application.Features.RoomFavorites.Queries.GetMyFavoriteRooms
{
    public sealed class GetMyFavoriteRoomsQueryHandler
    {
        #region fields

        private readonly IRepository<Room> _roomRepository;
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region constructor
        public GetMyFavoriteRoomsQueryHandler(ICurrentUserService currentUserService, IRepository<Room> roomRepository)
        {
            _currentUserService = currentUserService;
            _roomRepository = roomRepository;
        }
        #endregion

        #region handle functions       

        public async Task<Result<PagedResult<FavoriteRoomDto>>> HandleAsync( GetMyFavoriteRoomsQuery query, CancellationToken cancellationToken = default)
        {
            // 1. Get Current User

            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Result<PagedResult<FavoriteRoomDto>>.Failure( "AUTH_UNAUTHORIZED", "User is not authenticated.");
            }

            // 2. Validate Pagination

            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;

            var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 50);

            // 3. Calculate Skip

            var skip = (pageNumber - 1) * pageSize;

            // 4. Get Rooms

            var specification = new MyFavoriteRoomsSpecification( userId.Value, skip, pageSize);

            var rooms = await _roomRepository.ListAsync( specification, cancellationToken);

            // 5. Get Total Count

            var countSpecification = new MyFavoriteRoomsCountSpecification( userId.Value);

            var totalCount = await _roomRepository.CountAsync( countSpecification, cancellationToken);

            // 6. Map Rooms

            var items = rooms
                .Select(room => new FavoriteRoomDto
                {
                    Id = room.Id,

                    Title = room.Title,

                    City = room.City,

                    Area = room.Area,

                    MonthlyRent = room.MonthlyRent,

                    MainImageKey = room.Images
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => x.ImageKey)
                        .FirstOrDefault()
                })
                .ToList();

            // 7. Create Paged Result

            var pagedResult = new PagedResult<FavoriteRoomDto>
            {
                Items = items,

                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalCount = totalCount
            };

            // 8. Return Result

            return Result<PagedResult<FavoriteRoomDto>>
                .Success(pagedResult);
        }

        #endregion
    }
}