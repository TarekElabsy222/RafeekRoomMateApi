using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.UserProfiles
{
    public sealed class UserProfilesByUserIdsSpecification: BaseSpecification<UserProfile>
    {
        public UserProfilesByUserIdsSpecification(IReadOnlyCollection<Guid> userIds)
            : base(x => userIds.Contains(x.UserId))
        {
        }
    }
}
