namespace Doerly.Module.Communication.Domain.Constants;

public class CommunicationConstants
{
    public class FileConstants
    {
        public static readonly string[] SupportedFileExtensions =
        [
            "pdf",
            "docx",
            "jpg",
            "jpeg",
            "png"
        ];
        
        public static long MaxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB
    }

    public class FolderNames
    {
        public const string CommunicationFiles = "communication-files";
    }

    public class AzureStorage
    {
        public const string FilesContainerName = "files";
    }
}
