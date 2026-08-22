namespace RafeekRoomMate.Application.Common.Interfaces
{
    public interface IRoomImageValidator
    {
        Task<bool> IsValidAsync(
            Stream stream,
            string fileName,
            string contentType,
            long length,
            CancellationToken cancellationToken = default);
    }
}
