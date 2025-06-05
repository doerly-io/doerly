namespace Doerly.Common.Settings;

public class RedisSettings
{
    public const string RedisSettingName = nameof(RedisSettings);
    
    public string ConnectionString { get; set; }
}