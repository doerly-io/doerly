using Doerly.Common;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Localization;
using Doerly.Module.Authorization.Contracts.Dtos;
using Doerly.Module.Authorization.DataAccess.Entities;
using Doerly.Module.Authorization.Contracts.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class RegistrationHandler : BaseAuthHandler
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RegistrationHandler(
        AuthorizationDbContext dbContext,
        IOptions<AuthSettings> authOptions,
        IPublishEndpoint publishEndpoint
    ) : base(dbContext, authOptions)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<HandlerResult> HandleAsync(RegisterRequestDto requestDto)
    {
        var userExist = DbContext.Users.AsNoTracking().Any(x => x.Email == requestDto.Email);
        if (userExist)
            return HandlerResult.Failure(Resources.Get("UserAlreadyExist"));

        var (passwordHash, passwordSalt) = GetPasswordHash(requestDto.Password);

        var user = new UserEntity
        {
            Email = requestDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        await PublishUserRegisteredEventAsync(user.Id, user.Email, requestDto.FirstName, requestDto.LastName);

        return HandlerResult.Success();
    }

    private async Task PublishUserRegisteredEventAsync(int userId, string email, string firstName, string lastName)
    {
        var message = new UserRegisteredMessage(userId, email, firstName, lastName);
        await _publishEndpoint.Publish(message);
    }
}
