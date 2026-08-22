namespace RafeekRoomMate.Infrastructure.Storage
{
    public sealed class ProfileImageOptions
    {
        public long MaxSizeInBytes { get; set; }

        public string[] AllowedExtensions { get; set; } = [];
    }
}
