using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Domain.Specifications.Rooms
{
    public sealed class RoomsCountSpecification : BaseSpecification<Room>
    {
        public RoomsCountSpecification(
            string? city,
            string? area,
            decimal? minRent,
            decimal? maxRent,
            RoomType? roomType,
            GenderPreference? genderPreference)
            : base(x =>
                x.Status == RoomStatus.Available &&

                (string.IsNullOrEmpty(city) ||
                 x.City == city) &&

                (string.IsNullOrEmpty(area) ||
                 x.Area == area) &&

                (!minRent.HasValue ||
                 x.MonthlyRent >= minRent.Value) &&

                (!maxRent.HasValue ||
                 x.MonthlyRent <= maxRent.Value) &&

                (!roomType.HasValue ||
                 x.RoomType == roomType.Value) &&

                (!genderPreference.HasValue ||
                 x.GenderPreference == genderPreference.Value))
        {
        }
    }

}
