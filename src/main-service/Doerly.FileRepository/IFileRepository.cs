namespace Doerly.FileRepository;

public interface IFileRepository
{
    Task UploadFileAsync(string containerName, string fileName, Stream stream);
    Task<Stream> DownloadFileAsync(string containerName, string fileName);
    Task DeleteFileAsync(string containerName, string fileName);
}