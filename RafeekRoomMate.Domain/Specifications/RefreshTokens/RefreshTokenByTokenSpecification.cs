using RafeekRoomMate.Domain.Entities;

namespace RafeekRoomMate.Domain.Specifications.RefreshTokens
{
    public sealed class RefreshTokenByTokenSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenByTokenSpecification(string token): base(x => x.Token == token)
        {
        }
    }
}
