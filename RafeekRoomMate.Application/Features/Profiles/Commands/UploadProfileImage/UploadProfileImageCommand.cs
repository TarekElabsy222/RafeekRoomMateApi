namespace RafeekRoomMate.Application.Features.Profiles.Commands.UploadProfileImage
{
    public sealed class UploadProfileImageCommand
    {
        public Stream Stream { get; init; } = default!;

        public string FileName { get; init; } = string.Empty;

        public string ContentType { get; init; } = string.Empty;

        public long Length { get; init; }
    }

}
