using Doerly.Helpers;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Doerly.Module.Order.Tests;

public class ImageValidationHelperTests
{

    [Fact]
    public void IsValidImage_ReturnsFalse_WhenBytesAreNotImage()
    {
        // Arrange
        var bytes = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var result = ImageValidationHelper.IsValidImage(bytes, out var extension);

        // Assert
        Assert.False(result);
        Assert.Null(extension);
    }

    [Fact]
    public void IsValidImage_ReturnsTrue_ForValidPngImage()
    {
        // Arrange
        using var image = new Image<Rgba32>(10, 10);
        using var ms = new MemoryStream();
        image.Save(ms, new PngEncoder());
        var bytes = ms.ToArray();

        // Act
        var result = ImageValidationHelper.IsValidImage(bytes, out var extension);

        // Assert
        Assert.True(result);
        Assert.Equal("png", extension);
    }
}
