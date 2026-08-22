namespace RafeekRoomMate.Application.Common.Options
{
    public sealed class RoomImageOptions
    {
        public const string SectionName = "Storage:RoomImage";

        public long MaxSizeInBytes { get; set; } = 5 * 1024 * 1024;

        public int MaxImagesPerRoom { get; set; } = 5;

        public string[] AllowedExtensions { get; set; } =
        [
            ".jpg",
        ".jpeg",
        ".png",
        ".webp"
        ];
    }
}
