using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.UserProfiles
{
    public sealed class UserProfileByUserIdSpecification : BaseSpecification<UserProfile>
    {
        public UserProfileByUserIdSpecification(Guid userId)
            : base(x => x.UserId == userId)
        {
        }
    }
}
