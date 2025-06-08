using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Caching.Memory;

namespace Doerly.FileRepository;

/// <inheritdoc />
public class FileRepository : IFileRepository
{
    private const string StorageContainerResource = "c";
    private const string StorageBlobResource = "c";
    private const string StorageContainerSasTokenPrefix = "StorageContainerSasToken_";
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromHours(1);
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IMemoryCache _memoryCache;

    public FileRepository(BlobServiceClient blobServiceClient, IMemoryCache memoryCache)
    {
        _blobServiceClient = blobServiceClient;
        _memoryCache = memoryCache;
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

    public async Task<string> GetSasUrlAsync(string containerName, string fileName)
    {
        return await GetSasUrlAsync(containerName, fileName, DefaultExpiry);
    }

    public async Task<string> GetSasUrlAsync(string containerName, string fileName, TimeSpan expiry)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync())
            return string.Empty;

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = fileName,
            Resource = StorageBlobResource,
            ExpiresOn = DateTimeOffset.UtcNow.Add(expiry),
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var sasUri = blobClient.GenerateSasUri(sasBuilder);
        return sasUri.ToString();
    }

    public async Task<string> GetSasUrlToContainerAsync(string containerName, TimeSpan expiry)
    {
        var containerSasToken = await _memoryCache.GetOrCreateAsync(StorageContainerSasTokenPrefix + containerName, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = expiry;
            return await GenerateSasUrlToContainerAsync(containerName, expiry);
        });

        return containerSasToken;
    }

    private async Task<string> GenerateSasUrlToContainerAsync(string containerName, TimeSpan expiry)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        if (!await containerClient.ExistsAsync())
            return string.Empty;

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            Resource = StorageContainerResource,
            ExpiresOn = DateTimeOffset.UtcNow.Add(expiry),
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var sasUri = containerClient.GenerateSasUri(sasBuilder);
        return sasUri.ToString();
    }
}
