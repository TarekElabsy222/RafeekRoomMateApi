using RafeekRoomMate.Domain.Entities;
using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Domain.Specifications.Rooms
{

    public sealed class RoomsSpecification : BaseSpecification<Room>
    {
        public RoomsSpecification(
            string? city,
            string? area,
            decimal? minRent,
            decimal? maxRent,
            RoomType? roomType,
            GenderPreference? genderPreference,
            int pageNumber,
            int pageSize,
            RoomSortBy sortBy)
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
            // Include Room Images
            AddInclude(x => x.Images);

            // Include Room Members
            AddInclude(x => x.Members);

            // Sorting
            switch (sortBy)
            {
                case RoomSortBy.LowestRent:

                    ApplyOrderBy(x => x.MonthlyRent);

                    break;

                case RoomSortBy.HighestRent:

                    ApplyOrderByDescending(x => x.MonthlyRent);

                    break;

                case RoomSortBy.SoonestAvailable:

                    ApplyOrderBy(x => x.AvailableFrom);

                    break;

                case RoomSortBy.Newest:
                default:

                    ApplyOrderByDescending(x => x.CreatedAt);

                    break;
            }

            // Pagination
            ApplyPaging(
                (pageNumber - 1) * pageSize,
                pageSize);
        }
    }
}
