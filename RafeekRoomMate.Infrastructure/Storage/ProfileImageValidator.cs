using Microsoft.Extensions.Options;
using RafeekRoomMate.Application.Common.Interfaces;

namespace RafeekRoomMate.Infrastructure.Storage
{
    public class ProfileImageValidator : IImageValidator
    {
        #region fields
        private readonly ProfileImageOptions _options;
        #endregion

        #region constructor
        public ProfileImageValidator(IOptions<ProfileImageOptions> options)
        {
            _options = options.Value;
        }
        #endregion

        #region handle functions
        public async Task<bool> IsValidAsync( Stream stream, string fileName, string contentType, long length,  CancellationToken cancellationToken = default)
        {
            // 1. Check size
            if (length <= 0 || length > _options.MaxSizeInBytes)
            {
                return false;
            }

            // 2. Check extension
            var extension =
                Path.GetExtension(fileName)
                    .ToLowerInvariant();

            if (!_options.AllowedExtensions
                .Contains(extension))
            {
                return false;
            }

            // 3. Check actual file signature
            if (!stream.CanSeek)
            {
                return false;
            }

            stream.Position = 0;

            var header = new byte[12];

            var bytesRead = await stream.ReadAsync( header, cancellationToken);

            stream.Position = 0;

            return extension switch
            {
                ".jpg" or ".jpeg"  => IsJpeg(header, bytesRead),

                ".png"  => IsPng(header, bytesRead),

                ".webp" => IsWebp(header, bytesRead),

                _ => false
            };
        }

        private static bool IsJpeg(
            byte[] header,
            int length)
        {
            return length >= 3 && header[0] == 0xFF && header[1] == 0xD8 &&   header[2] == 0xFF;
        }

        private static bool IsPng( byte[] header, int length)
        {
            return length >= 8 &&
                   header[0] == 0x89 && header[1] == 0x50 &&
                   header[2] == 0x4E && header[3] == 0x47 &&
                   header[4] == 0x0D && header[5] == 0x0A &&
                   header[6] == 0x1A && header[7] == 0x0A;
        }

        private static bool IsWebp(
            byte[] header,
            int length)
        {
            return length >= 12 &&
                   header[0] == 'R' && header[1] == 'I' &&
                   header[2] == 'F' && header[3] == 'F' &&
                   header[8] == 'W' && header[9] == 'E' &&
                   header[10] == 'B' && header[11] == 'P';
        }
        #endregion
    }
}
