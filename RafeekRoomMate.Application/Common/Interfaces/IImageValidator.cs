namespace RafeekRoomMate.Application.Common.Interfaces
{
    public interface IImageValidator
    {
        Task<bool> IsValidAsync(Stream stream, string fileName,string contentType, long length,CancellationToken cancellationToken = default);
    }
}
