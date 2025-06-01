using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;

using Microsoft.EntityFrameworkCore;
using Doerly.Domain;
using Microsoft.AspNetCore.Http;
using Doerly.FileRepository;
using Doerly.Module.Profile.Domain.Constants;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateOrderHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public UpdateOrderHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext,
        IFileRepository fileRepository) : base(dbContext, fileRepository)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult> HandleAsync(int id, UpdateOrderRequest dto, List<IFormFile> files, List<string> existingFileNames)
    {
        var order = await DbContext.Orders.Include(order => order.OrderFiles)
            .FirstOrDefaultAsync(x => x.Id == id && x.CustomerId == _doerlyRequestContext.UserId);

        if (order == null)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        order.CategoryId = dto.CategoryId;
        order.Name = dto.Name;
        order.Description = dto.Description;
        order.Price = dto.Price;
        order.PaymentKind = dto.PaymentKind;
        order.DueDate = dto.DueDate;
        order.IsPriceNegotiable = dto.IsPriceNegotiable;

        var filesToDelete = order.OrderFiles
            .Where(f => !existingFileNames.Contains(f.FileName))
            .ToList();

        await DeleteOrderFilesAsync(order, filesToDelete);

        if (files != null && files.Count != 0)
            await UploadOrderFilesAsync(order, files);

        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
