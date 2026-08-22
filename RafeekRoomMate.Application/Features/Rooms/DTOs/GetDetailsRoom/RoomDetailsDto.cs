using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Application.Features.Rooms.DTOs.GetDetailsRoom
{
    public sealed class RoomDetailsDto
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public string City { get; init; } = string.Empty;

        public string Area { get; init; } = string.Empty;

        public decimal MonthlyRent { get; init; }

        public DateTime? AvailableFrom { get; init; }

        public int MaximumOccupants { get; init; }

        public RoomType RoomType { get; init; }

        public GenderPreference GenderPreference { get; init; }

        public RoomStatus Status { get; init; }

        public DateTime CreatedAt { get; init; }

        public RoomCreatorDto Creator { get; init; } = default!;

        public IReadOnlyCollection<RoomImageDto> Images { get; init; } = [];

        public IReadOnlyCollection<RoommateDto> Roommates { get; init; } = [];
    }
}
