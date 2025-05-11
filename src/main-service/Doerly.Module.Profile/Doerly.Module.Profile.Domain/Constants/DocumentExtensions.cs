namespace Doerly.Module.Profile.Domain.Constants;

// TODO: Can be replaced with admin settings in the future
public static class DocumentExtensions
{
    public const string Pdf = ".pdf";
    public const string Docx = ".docx";
    
    public static readonly IReadOnlyCollection<string> SupportedExtensions = new[] 
    {
        Pdf,
        Docx
    };
}