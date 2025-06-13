
using Doerly.Domain.Handlers;
using Doerly.Module.Notification.DataAccess;

namespace Doerly.Module.Notification.Domain.Handlers;

public abstract class BaseNotificationHandler(NotificationDbContext dbContext) : BaseHandler<NotificationDbContext>(dbContext);