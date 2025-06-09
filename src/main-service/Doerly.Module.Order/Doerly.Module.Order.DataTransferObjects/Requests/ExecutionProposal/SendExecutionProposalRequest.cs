using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.DataTransferObjects.Requests;

public class SendExecutionProposalRequest
{
    [Required(ErrorMessage = "FieldIsRequired")]
    public int OrderId { get; set; }

    [MaxLength(1000, ErrorMessage = "FieldIsTooLong")]
    public string Comment { get; set; } = string.Empty;

    [Required(ErrorMessage = "FieldIsRequired")]
    public int ReceiverId { get; set; }
}
