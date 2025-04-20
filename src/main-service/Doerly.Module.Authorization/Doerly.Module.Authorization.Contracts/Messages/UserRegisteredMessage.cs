namespace Doerly.Module.Authorization.Contracts.Messages;

public record UserRegisteredMessage(int UserId, string Email, string FirstName, string LastName);
