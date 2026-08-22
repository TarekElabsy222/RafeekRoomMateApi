using RafeekRoomMate.Application.Common.Models;
using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Application.Features.Rooms.DTOs.GetRooms;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.Rooms;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Rooms.Queries.GetRooms
{
    public sealed class GetRoomsQueryHandler
    {
        #region fields

        private readonly IRepository<Room> _roomRepository;

        private readonly IRepository<UserProfile> _userProfileRepository;

        private readonly IIdentityService _identityService;

        #endregion

        #region constructor
        public GetRoomsQueryHandler(
            IRepository<Room> roomRepository,
            IRepository<UserProfile> userProfileRepository,
            IIdentityService identityService)
        {
            _roomRepository = roomRepository;
            _userProfileRepository = userProfileRepository;
            _identityService = identityService;
        }
        #endregion

        #region handle

        public async Task<Result<PagedResult<RoomListItemDto>>> HandleAsync(GetRoomsQuery query, CancellationToken cancellationToken = default)
        {
            // 1. Validate Pagination

            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;

            var pageSize = query.PageSize < 1 ? 10 : Math.Min(query.PageSize, 50);

            // 2. Create Specification

            var specification =
                new RoomsSpecification(
                    query.City,
                    query.Area,
                    query.MinRent,
                    query.MaxRent,
                    query.RoomType,
                    query.GenderPreference,
                    pageNumber,
                    pageSize,
                    query.SortBy);

            // 3. Get Rooms
            var rooms =await _roomRepository.ListAsync(specification,cancellationToken);

            // 4. Get Total Count

            var countSpecification =new RoomsCountSpecification(query.City, query.Area, query.MinRent, query.MaxRent, query.RoomType, query.GenderPreference);

            var totalCount = await _roomRepository.CountAsync(countSpecification, cancellationToken);

            // 5. If no rooms

            if (rooms.Count == 0)
            {
                var emptyResult = new PagedResult<RoomListItemDto>
                {
                    Items = [],
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };

                return Result<PagedResult<RoomListItemDto>>.Success(emptyResult);
            }

            // 6. Get Creator IDs

            var creatorIds = rooms.Select(x => x.CreatedByUserId).Distinct().ToList();

            // 7. Get Users in One Call

            var users = await _identityService.GetUsersAsync(creatorIds, cancellationToken);

            var usersDictionary = users.ToDictionary(x => x.Id);

            // 8. Get Profiles in One Query

            var profileSpecification = new UserProfilesByUserIdsSpecification(creatorIds);

            var profiles = await _userProfileRepository.ListAsync(profileSpecification,cancellationToken);

            var profilesDictionary = profiles.ToDictionary(x => x.UserId);

            // 9. Map Rooms

            var items = rooms.Select(room =>
            {
                usersDictionary.TryGetValue(room.CreatedByUserId, out var creator);

                profilesDictionary.TryGetValue(room.CreatedByUserId,out var profile);

                // Current active occupants
                var currentOccupants = room.Members.Count(x => x.IsActive);

                // First image = Cover Image
                var coverImage =room.Images.OrderBy(x => x.DisplayOrder).FirstOrDefault();

                return new RoomListItemDto
                {
                    Id = room.Id,

                    Title = room.Title,

                    City = room.City,

                    Area = room.Area,

                    MonthlyRent = room.MonthlyRent,

                    AvailableFrom = room.AvailableFrom,

                    MaximumOccupants = room.MaximumOccupants,

                    CurrentOccupants = currentOccupants,

                    RoomType = room.RoomType,

                    GenderPreference = room.GenderPreference,

                    CoverImageKey = coverImage?.ImageKey,

                    CreatorId = room.CreatedByUserId,

                    CreatorName = creator is null? string.Empty: $"{creator.FirstName} {creator.LastName}",

                    CreatorProfileImageKey = profile?.ProfileImageKey
                };
            }).ToList();

            // 10. Build Paged Result

            var result = new PagedResult<RoomListItemDto>
            {
                Items = items,

                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalCount = totalCount
            };

            // 11. Return

            return Result<PagedResult<RoomListItemDto>>.Success(result);
        }

        #endregion

    }
}
