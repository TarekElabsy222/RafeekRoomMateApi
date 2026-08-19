namespace RafeekRoomMate.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(Stream stream,string fileName, string contentType,string folder, CancellationToken cancellationToken = default);

        Task DeleteAsync( string fileKey, CancellationToken cancellationToken = default);
    }
}
