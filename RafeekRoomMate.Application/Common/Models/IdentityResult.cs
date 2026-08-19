namespace RafeekRoomMate.Application.Common.Models
{
    public class AuthResult
    {
        public bool Succeeded { get; init; }

        public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

        public Guid? UserId { get; init; }

        public static AuthResult Success(Guid userId)
        {
            return new AuthResult
            {
                Succeeded = true,
                UserId = userId
            };
        }

        public static AuthResult Failure(
            IEnumerable<string> errors)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = errors.ToArray()
            };
        }
    }
}
