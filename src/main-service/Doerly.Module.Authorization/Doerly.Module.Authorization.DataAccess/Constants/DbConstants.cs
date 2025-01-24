namespace Doerly.Module.Authorization.DataAccess.Constants;

internal class DbConstants
{
    internal const string AuthSchema = "authorization";

    internal class Tables
    {
        internal const string User = "user";
        internal const string Role = "role";
        internal const string RefreshToken = "refresh_token";
    }
}