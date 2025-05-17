namespace Doerly.Module.Profile.Domain.Helpers;

using Constants;

public class DocumentValidationHelper
{

    public static bool IsValidDocument(byte[] documentBytes, out string fileExtension)
    {
        fileExtension = null;
        
        if (documentBytes == null || documentBytes.Length == 0)
            return false;
        
        try
        {
            // PDF validation - check for PDF signature
            if (IsPdf(documentBytes))
            {
                fileExtension = DocumentExtensions.Pdf;
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    private static bool IsPdf(byte[] bytes)
    {
        // PDF files start with %PDF
        return bytes.Length >= 4 && 
               bytes[0] == 0x25 && // %
               bytes[1] == 0x50 && // P
               bytes[2] == 0x44 && // D
               bytes[3] == 0x46;   // F
    }
    
}