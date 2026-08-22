using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Application.Features.Rooms.Commands.CreateRoom
{
    public sealed class CreateRoomCommand
    {
        public string Title { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public string City { get; init; } = string.Empty;

        public string Area { get; init; } = string.Empty;

        public decimal MonthlyRent { get; init; }

        public DateTime? AvailableFrom { get; init; }

        public int MaximumOccupants { get; init; }

        public RoomType RoomType { get; init; }

        public GenderPreference GenderPreference { get; init; }
    }
}
