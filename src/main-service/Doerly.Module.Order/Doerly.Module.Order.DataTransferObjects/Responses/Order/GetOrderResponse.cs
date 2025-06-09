using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.Enums;
using FileInfo = Doerly.Module.Order.DataTransferObjects.FileInfo;

namespace Doerly.Module.Order.DataTransferObjects;
public class GetOrderResponse
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public bool IsPriceNegotiable { get; set; }

    public EPaymentKind PaymentKind { get; set; }

    public DateTime DueDate { get; set; }

    public EOrderStatus Status { get; set; }

    public int CustomerId { get; set; }

    public ProfileInfo Customer { get; set; }

    public bool CustomerCompletionConfirmed { get; set; }

    public int? ExecutorId { get; set; }

    public ProfileInfo? Executor { get; set; }

    public bool ExecutorCompletionConfirmed { get; set; }

    public DateTime? ExecutionDate { get; set; }

    public int? BillId { get; set; }

    public bool UseProfileAddress { get; set; }

    public AddressInfo AddressInfo { get; set; }

    public IEnumerable<FileInfo> ExistingFiles { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public OrderFeedbackResponse? Feedback { get; set; }
}
