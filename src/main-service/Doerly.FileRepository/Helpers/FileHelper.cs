namespace Doerly.FileRepository;

public static class FileHelper
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

    /// <summary>
    /// Checks if the file has a valid size and extension.
    /// </summary>
    /// <param name="fileName">File name (with extension).</param>
    /// <param name="fileBytes">File content as bytes.</param>
    /// <param name="allowedExtensions">List of allowed extensions (without dot).</param>
    /// <param name="maxFileSizeInBytes">Maximum file size in bytes.</param>
    /// <returns>True if the file is valid, otherwise false.</returns>
    public static bool IsValidFile(string fileName, byte[] fileBytes, IEnumerable<string> allowedExtensions, long maxFileSizeInBytes)
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


    /// <summary>
    /// Gets the file extension (with dot) from the given file name.
    /// </summary>
    /// <param name="fileName">The file name to extract the extension from.</param>
    /// <returns>The file extension including the dot, or an empty string if none found.</returns>
    public static string GetFileExtension(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        var ext = Path.GetExtension(fileName);
        return ext ?? string.Empty;
    }

    private static string? DetectFileExtension(byte[] fileBytes)
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