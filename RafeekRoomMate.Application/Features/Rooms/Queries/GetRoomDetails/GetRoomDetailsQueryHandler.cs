using RafeekRoomMate.Application.Common.Results;
using RafeekRoomMate.Application.Features.Authentication.Common;
using RafeekRoomMate.Application.Features.Rooms.DTOs;
using RafeekRoomMate.Application.Features.Rooms.DTOs.GetDetailsRoom;
using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Repositories.Interfaces;
using RafeekRoomMate.Domain.Specifications.Rooms;
using RafeekRoomMate.Domain.Specifications.UserProfiles;

namespace RafeekRoomMate.Application.Features.Rooms.Queries.GetRoomDetails
{
    public class GetRoomDetailsQueryHandler
    {
        #region fiels
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IIdentityService _identityService;
        #endregion

        #region constructor
        public GetRoomDetailsQueryHandler(IRepository<Room> roomRepository, IIdentityService identityService, IRepository<UserProfile> userProfileRepository)
        {
            _roomRepository = roomRepository;
            _identityService = identityService;
            _userProfileRepository = userProfileRepository;
        }
        #endregion

        #region handle functions
        public async Task<Result<RoomDetailsDto>> HandleAsync(GetRoomDetailsQuery query, CancellationToken cancellationToken = default)
        {
            // 1. Get Room + Images + Members

            var specification = new RoomDetailsSpecification(query.RoomId);

            var room = await _roomRepository.GetBySpecificationAsync(specification,cancellationToken);
            if (room is null)
            {
                return Result<RoomDetailsDto>.Failure("ROOM_NOT_FOUND","Room was not found.");
            }

            // 2. Get Current Roommates

            var activeMembers = room.Members.Where(x => x.IsActive).ToList();

            // 2. Collect User IDs

            var userIds = room.Members.Where(x => x.IsActive)
                .Select(x => x.UserId).Append(room.CreatedByUserId)
                .Distinct().ToList();

            // 3. Get all users with ONE query

            var users = await _identityService.GetUsersAsync(userIds, cancellationToken);

            var usersDictionary =users.ToDictionary(x => x.Id);

            // 4. Get Creator

            if (!usersDictionary.TryGetValue(room.CreatedByUserId,out var creatorUser))
            {
                return Result<RoomDetailsDto>.Failure("ROOM_CREATOR_NOT_FOUND","Room creator was not found.");
            }

            // 6. Get Profiles
            var profileSpecification =new UserProfilesByUserIdsSpecification(userIds);

            var profiles =  await _userProfileRepository.ListAsync(profileSpecification,cancellationToken);

            var profilesDictionary = profiles.ToDictionary(x => x.UserId);
            // 5. Creator Profile Image            


            profilesDictionary.TryGetValue(room.CreatedByUserId, out var creatorProfile);
            var creator = new RoomCreatorDto
            {
                Id = creatorUser.Id,

                Name = $"{creatorUser.FirstName} {creatorUser.LastName}",

                Bio = creatorProfile?.Bio ?? string.Empty,

                ProfileImageKey = creatorProfile?.ProfileImageKey
            };

            // 6. Map Room Images

            var images = room.Images.OrderBy(x => x.DisplayOrder).Select(x => new RoomImageDto
                {
                    Id = x.Id,
                    ImageKey = x.ImageKey,
                    DisplayOrder = x.DisplayOrder
                }).ToList();

            // 7. Map Current Roommates

            var roommates = activeMembers.Where(x => usersDictionary.ContainsKey(x.UserId))
                .Select(x =>
                {
                    var user = usersDictionary[x.UserId];

                    profilesDictionary.TryGetValue(x.UserId, out var profile);

                    return new RoommateDto
                    {
                        Id = user.Id,

                        Name =
                            $"{user.FirstName} {user.LastName}",

                        Bio =
                            profile?.Bio ?? string.Empty,

                        ProfileImageKey =
                            profile?.ProfileImageKey,

                        JoinedAt =
                            x.JoinedAt
                    };
                }).OrderBy(x => x.JoinedAt).ToList();

            // 8. Build Room Details

            var response = new RoomDetailsDto
            {
                Id = room.Id,
                Title = room.Title,
                Description = room.Description,
                City = room.City,
                Area = room.Area,
                MonthlyRent = room.MonthlyRent,
                AvailableFrom = room.AvailableFrom,
                MaximumOccupants = room.MaximumOccupants,
                RoomType = room.RoomType,
                GenderPreference = room.GenderPreference,
                Status = room.Status,
                CreatedAt = room.CreatedAt,
                Creator = creator,
                Images = images,
                Roommates = roommates
            };

            return Result<RoomDetailsDto>.Success(response);

        }
        #endregion
        }
}
