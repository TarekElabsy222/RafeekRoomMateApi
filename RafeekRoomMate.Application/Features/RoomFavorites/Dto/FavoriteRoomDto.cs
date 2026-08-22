namespace RafeekRoomMate.Application.Features.RoomFavorites.Dto
{
    public sealed class FavoriteRoomDto
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string City { get; init; } = string.Empty;

        public string Area { get; init; } = string.Empty;

        public decimal MonthlyRent { get; init; }

        public string? MainImageKey { get; init; }

        public DateTime FavoritedAt { get; init; }
    }
}
