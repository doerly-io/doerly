using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Doerly.Common.Settings;

public class FrontendSettings
{
    public const string FrontendSettingsName = nameof(FrontendSettings);
    
    [Required]
    public string FrontendUrl { get; set; }
}
