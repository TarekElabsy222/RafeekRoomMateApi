using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Application.Features.Rooms.DTOs.GetRooms
{
    public sealed class RoomListItemDto
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string City { get; init; } = string.Empty;

        public string Area { get; init; } = string.Empty;

        public decimal MonthlyRent { get; init; }

        public DateTime? AvailableFrom { get; init; }

        public int MaximumOccupants { get; init; }

        public int CurrentOccupants { get; init; }

        public RoomType RoomType { get; init; }

        public GenderPreference GenderPreference { get; init; }

        public string? CoverImageKey { get; init; }

        public Guid CreatorId { get; init; }

        public string CreatorName { get; init; } = string.Empty;

        public string? CreatorProfileImageKey { get; init; }
    }
}
