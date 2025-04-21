namespace Doerly.FileRepository;

/// <summary>
/// Service for working with files in Azure Blob Storage
/// </summary>
public interface IFileRepository
{
    /// <summary>
    /// Uploads a file to the specified container (if the file already exists, it will be overwritten)
    /// </summary>
    Task UploadFileAsync(string containerName, string fileName, byte[] fileBytes);

    /// <summary>
    /// Uploads a file to the specified container (if the file already exists, it will be overwritten)
    /// A blob can have up to 10 tags. Tag keys must be between 1 and 128 characters.
    /// Tag values must be between 0 and 256 characters.
    /// Valid tag key and value characters include lower and upper case letters,
    /// digits (0-9), space (' '), plus ('+'), minus ('-'), period ('.'), forward slash ('/'), colon (':'), equals ('='), and underscore ('_').
    /// </summary>
    Task UploadFileAsync(string containerName, string fileName, byte[] fileBytes, Dictionary<string, string> blobTags);

    /// <summary>
    /// Uploads a file to the specified container (if the file already exists, it will be overwritten)
    /// </summary>
    Task UploadFileAsync(string containerName, string fileName, Stream stream);

    /// <summary>
    /// Uploads a file to the specified container (if the file already exists, it will be overwritten)
    /// A blob can have up to 10 tags. Tag keys must be between 1 and 128 characters.
    /// Tag values must be between 0 and 256 characters.
    /// Valid tag key and value characters include lower and upper case letters,
    /// digits (0-9), space (' '), plus ('+'), minus ('-'), period ('.'), forward slash ('/'), colon (':'), equals ('='), and underscore ('_').
    /// </summary>
    Task UploadFileAsync(string containerName, string fileName, Stream stream, Dictionary<string, string> blobTags);

    /// <summary>
    /// Downloads a file from the specified container
    /// </summary>
    /// <remarks>
    /// A RequestFailedException will be thrown if a failure occurs. If multiple failures occur, an AggregateException will be thrown, containing each failure instance.
    /// </remarks>
    /// <returns>Byte array of the file</returns>
    Task<byte[]> DownloadFileBytesAsync(string containerName, string fileName);

    /// <summary>
    /// Deletes a file if it exists from the specified container
    /// </summary>
    /// <returns></returns>
    Task DeleteFileIfExistsAsync(string containerName, string fileName);

    /// <summary>
    /// Gets a SAS URL for the specified file in the specified container with the default expiry time of 1 hour
    /// </summary>
    Task<string?> GetSasUrlAsync(string containerName, string fileName);
        
    /// <summary>
    /// Gets a SAS URL for the specified file in the specified container with the specified expiry time
    /// </summary>
    Task<string> GetSasUrlAsync(string containerName, string fileName, TimeSpan expiry);

}
