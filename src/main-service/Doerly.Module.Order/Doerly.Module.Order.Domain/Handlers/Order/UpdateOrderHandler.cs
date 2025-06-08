using Doerly.Domain;
using Doerly.Domain.Exceptions;
using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Domain.Constants;
using Doerly.Proxy.Profile;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateOrderHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public UpdateOrderHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext,
        IFileRepository fileRepository, IProfileModuleProxy profileModuleProxy) : base(dbContext, fileRepository, profileModuleProxy)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult> HandleAsync(int id, UpdateOrderRequest dto, List<IFormFile> files, List<string> existingFileNames)
    {
        var userId = _doerlyRequestContext.UserId ?? throw new DoerlyException("We are fucked!");

        (int regionId, int cityId) = await ManageAddress(userId, dto.UseProfileAddress, dto.RegionId, dto.CityId);

        var order = await DbContext.Orders.Include(order => order.OrderFiles)
            .FirstOrDefaultAsync(x => x.Id == id && x.CustomerId == userId);

        if (order == null)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        order.Name = dto.Name;
        order.Description = dto.Description;
        order.Price = dto.Price;
        order.PaymentKind = dto.PaymentKind;
        order.DueDate = dto.DueDate;
        order.IsPriceNegotiable = dto.IsPriceNegotiable;
        order.RegionId = regionId;
        order.CityId = cityId;

        foreach (var orderFile in order.OrderFiles)
        {
            if (!existingFileNames.Contains(orderFile.Name))
                await DeleteOrderFileAsync(orderFile);
        }

        if (files != null && files.Count != 0)
        {
            foreach (var file in files)
            {
                var orderFile = await CreateOrderFileAsync(order.Code, file);
                if (orderFile != null)
                    order.OrderFiles.Add(orderFile);
            }
        }

        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }

    private async Task DeleteOrderFileAsync(OrderFile file)
    {
        await DeleteOrderFileFromStorageAsync(file);

        DbContext.OrderFiles.Remove(file);
    }
}
