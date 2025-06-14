namespace Doerly.Module.Communication.DataTransferObjects.Messages;

public record NewMessageNotificationMessage(
    int UserId,
    string Data
); 