namespace Doerly.Module.Profile.Domain.Constants;

public class ImageExtensions
{
    public const string Jpg = ".jpg";
    public const string Png = ".png";
    public const string Jpeg = ".jpeg";
    
    public static readonly IReadOnlyCollection<string> SupportedExtensions = new[] 
    {
        Jpg,
        Png,
        Jpeg
    };
}
