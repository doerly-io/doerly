using System.Text;
using Doerly.Module.Profile.Domain.Constants;
using Doerly.Module.Profile.Domain.Helpers;

namespace Doerly.Module.Profile.Tests;

public class DocumentValidationHelperTests
{
    [Fact]
    public void IsValidDocument_WithValidPdf_ReturnsTrue()
    {
        // Arrange
        // Create a minimal valid PDF content with the PDF header signature
        byte[] pdfBytes = Encoding.ASCII.GetBytes("%PDF-1.5\n Some PDF content");
        
        // Act
        bool isValid = DocumentValidationHelper.IsValidDocument(pdfBytes, out string fileExtension);
        
        // Assert
        Assert.True(isValid);
        Assert.Equal(DocumentExtensions.Pdf, fileExtension);
    }
    
    [Fact]
    public void IsValidDocument_WithNullBytes_ReturnsFalse()
    {
        // Act
        bool isValid = DocumentValidationHelper.IsValidDocument(null, out string fileExtension);
        
        // Assert
        Assert.False(isValid);
        Assert.Null(fileExtension);
    }
    
    [Fact]
    public void IsValidDocument_WithEmptyBytes_ReturnsFalse()
    {
        // Arrange
        byte[] emptyBytes = Array.Empty<byte>();
        
        // Act
        bool isValid = DocumentValidationHelper.IsValidDocument(emptyBytes, out string fileExtension);
        
        // Assert
        Assert.False(isValid);
        Assert.Null(fileExtension);
    }
    
    [Fact]
    public void IsValidDocument_WithNonPdfDocument_ReturnsFalse()
    {
        // Arrange
        // Some non-PDF content
        byte[] nonPdfBytes = Encoding.UTF8.GetBytes("This is not a PDF file");
        
        // Act
        bool isValid = DocumentValidationHelper.IsValidDocument(nonPdfBytes, out string fileExtension);
        
        // Assert
        Assert.False(isValid);
        Assert.Null(fileExtension);
    }
    
    [Fact]
    public void IsValidDocument_WithTooShortBytes_ReturnsFalse()
    {
        // Arrange
        // Only 3 bytes (less than the 4 needed for PDF signature check)
        byte[] tooShortBytes = new byte[] { 0x25, 0x50, 0x44 }; // %PD
        
        // Act
        bool isValid = DocumentValidationHelper.IsValidDocument(tooShortBytes, out string fileExtension);
        
        // Assert
        Assert.False(isValid);
        Assert.Null(fileExtension);
    }
    
    [Fact]
    public void IsValidDocument_WithPartialPdfSignature_ReturnsFalse()
    {
        // Arrange
        // First 3 bytes match PDF signature but 4th doesn't
        byte[] partialPdfSignature = new byte[] { 0x25, 0x50, 0x44, 0x00 }; // %PD<null>
        
        // Act
        bool isValid = DocumentValidationHelper.IsValidDocument(partialPdfSignature, out string fileExtension);
        
        // Assert
        Assert.False(isValid);
        Assert.Null(fileExtension);
    }
}
