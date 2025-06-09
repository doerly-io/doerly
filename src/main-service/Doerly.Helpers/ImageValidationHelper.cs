using SixLabors.ImageSharp;

namespace Doerly.Helpers;

public class ImageValidationHelper
{
    public static bool IsValidImage(byte[] imageBytes, out string fileExtension)
    {
        fileExtension = null;
        if (imageBytes == null || imageBytes.Length == 0)
            return false;

        try
        {
            using var image = Image.Load(imageBytes);
            if (image.Width == 0 || image.Height == 0)
                return false;

            var format = Image.DetectFormat(imageBytes);
            fileExtension = format.FileExtensions.First();

            return true;
        }
        catch
        {
            return false;
        }
    }
}
