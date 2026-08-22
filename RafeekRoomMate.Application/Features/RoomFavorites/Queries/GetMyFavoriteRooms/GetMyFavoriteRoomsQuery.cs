namespace RafeekRoomMate.Application.Features.RoomFavorites.Queries.GetMyFavoriteRooms
{
    public sealed class GetMyFavoriteRoomsQuery
    {
        public int PageNumber { get; init; } = 1;

        public int PageSize { get; init; } = 10;
    }
}
