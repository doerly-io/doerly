namespace Doerly.Extensions;

public static class StringExtensions
{
    public static string ToControllerName(this string controllerName)
    {
        return controllerName.EndsWith("Controller") ? controllerName[..^"Controller".Length] : controllerName;
    }
}
