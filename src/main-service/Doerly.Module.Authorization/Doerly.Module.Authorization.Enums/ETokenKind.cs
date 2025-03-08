namespace Doerly.Module.Authorization.Enums;

public enum ETokenKind : byte
{
    PasswordReset = 1,
    EmailConfirmation = 2,
    RefreshToken = 3
}
