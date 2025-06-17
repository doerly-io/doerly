namespace Doerly.Module.Authorization.DataTransferObjects.Messages;

public record UserRegisteredMessage(int UserId, string Email, string FirstName, string LastName);
