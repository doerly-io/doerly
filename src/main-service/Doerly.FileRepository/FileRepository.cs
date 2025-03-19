using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace Doerly.FileRepository;

/// <inheritdoc />
public class FileRepository : IFileRepository
{
    private readonly BlobServiceClient _blobServiceClient;

    public FileRepository(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task UploadFileAsync(string containerName, string fileName, byte[] fileBytes, Dictionary<string, string> blobTags)
    {
        using (var memoryStream = new MemoryStream(fileBytes))
        {
            await UploadFileAsync(containerName, fileName, memoryStream, blobTags);
        }
    }

    public async Task UploadFileAsync(string containerName, string fileName, byte[] fileBytes)
    {
        using (var memoryStream = new MemoryStream(fileBytes))
        {
            await UploadFileAsync(containerName, fileName, memoryStream);
        }
    }

    public async Task UploadFileAsync(string containerName, string fileName, Stream stream)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(stream, true);
    }

    public async Task UploadFileAsync(string containerName, string fileName, Stream stream, Dictionary<string, string> blobTags)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(stream, true);
        await blobClient.SetTagsAsync(blobTags);
    }

    public async Task<byte[]> DownloadFileBytesAsync(string containerName, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        using (var memoryStream = new MemoryStream())
        {
            await blobClient.DownloadToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public async Task DeleteFileIfExistsAsync(string containerName, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<string?> GetSasUrlAsync(string containerName, string fileName, TimeSpan? expiry = null)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync())
        {
            return null;
        }

        expiry ??= TimeSpan.FromHours(1);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = fileName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(expiry.Value)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blobClient.GenerateSasUri(sasBuilder);

        return sasUri.ToString();
    }
}
