using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Application.Features.Rooms.Queries.GetRooms
{
    public sealed class GetRoomsQuery
    {
        public int PageNumber { get; init; } = 1;

        public int PageSize { get; init; } = 10;

        public string? City { get; init; }

        public string? Area { get; init; }

        public decimal? MinRent { get; init; }

        public decimal? MaxRent { get; init; }

        public RoomType? RoomType { get; init; }

        public GenderPreference? GenderPreference { get; init; }

        public RoomSortBy SortBy { get; init; } = RoomSortBy.Newest;
    }
}
