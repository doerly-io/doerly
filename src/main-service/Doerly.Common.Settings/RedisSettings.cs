using System.ComponentModel.DataAnnotations;

namespace Doerly.Common.Settings;

public class RedisSettings
{
    public const string RedisSettingName = nameof(RedisSettings);
    
    public const string RedisInstanceName = "Doerly:";
    
    [Required]
    public string ConnectionString { get; set; }
}
