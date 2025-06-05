using Microsoft.AspNetCore.Http;

namespace Doerly.FileRepository;

public interface IFileHelper
{
    /// <summary>
    /// Checks if the file has a valid size and extension.
    /// </summary>
    /// <param name="fileName">File name (with extension).</param>
    /// <param name="fileBytes">File content as bytes.</param>
    /// <param name="allowedExtensions">List of allowed extensions (without dot).</param>
    /// <param name="maxFileSizeInBytes">Maximum file size in bytes.</param>
    /// <returns>True if the file is valid, otherwise false.</returns>
    bool IsValidFile(string fileName, byte[] fileBytes, IEnumerable<string> allowedExtensions, long maxFileSizeInBytes);

    /// <summary>
    /// Converts an <see cref="IFormFile"/> to a byte array.
    /// </summary>
    /// <param name="file">The uploaded form file.</param>
    /// <returns>The file content as a byte array.</returns>
    Task<byte[]> GetFormFileBytesAsync(IFormFile file);
    
    /// <summary>
    /// Gets the file extension (with dot) from the given file name.
    /// </summary>
    /// <param name="fileName">The file name to extract the extension from.</param>
    /// <returns>The file extension including the dot, or an empty string if none found.</returns>
    string GetFileExtension(string fileName);
}