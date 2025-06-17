namespace Doerly.Common.Settings;

public class AzureStorageSettings
{
    public const string AzureStorageSettingName = nameof(AzureStorageSettings);
    
    public string ConnectionString { get; set; }
    
    public string ProfileImagesContainerName { get; set; }
}
