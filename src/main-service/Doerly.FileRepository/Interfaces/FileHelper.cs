using Microsoft.AspNetCore.Http;

namespace Doerly.FileRepository;

public class FileHelper : IFileHelper
{
    // Basic magic numbers for common file types
    private static readonly Dictionary<string, byte[]> MagicNumbers = new()
    {
        { "jpg", [0xFF, 0xD8, 0xFF] },
        { "png", [0x89, 0x50, 0x4E, 0x47] },
        { "pdf", [0x25, 0x50, 0x44, 0x46] },
        { "doc", [0xD0, 0xCF, 0x11, 0xE0] },
        { "docx", [0x50, 0x4B, 0x03, 0x04] }, // Also zip
        { "xlsx", [0x50, 0x4B, 0x03, 0x04] },
        { "zip", [0x50, 0x4B, 0x03, 0x04] },
    };

    public bool IsValidFile(string fileName, byte[] fileBytes, IEnumerable<string> allowedExtensions, long maxFileSizeInBytes)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileBytes.Length == 0)
            return false;

        if (fileBytes.Length > maxFileSizeInBytes)
            return false;

        var fileExt = Path.GetExtension(fileName)?.TrimStart('.').ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExt))
            return false;

        var allowed = allowedExtensions.Select(e => e.ToLowerInvariant()).ToList();

        if (!allowed.Contains(fileExt))
            return false;

        var detectedExt = DetectFileExtension(fileBytes);
        if (detectedExt == null)
            return false;

        if (!allowed.Contains(detectedExt))
            return false;

        return fileExt == detectedExt;
    }

    public async Task<byte[]> GetFormFileBytesAsync(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        return stream.ToArray();
    }

    public string GetFileExtension(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        var ext = Path.GetExtension(fileName);
        return ext ?? string.Empty;
    }

    private string? DetectFileExtension(byte[] fileBytes)
    {
        foreach (var (key, signature) in MagicNumbers)
        {
            if (fileBytes.Length >= signature.Length &&
                fileBytes.Take(signature.Length).SequenceEqual(signature))
            {
                return key;
            }
        }
        return null;
    }
}