using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.DataTransferObjects.Dtos;

public class SendExecutionProposalRequest
{
    [Required(ErrorMessage = "FieldIsRequired")]
    public int OrderId { get; set; }

    [MaxLength(1000, ErrorMessage = "FieldIsTooLong")]
    public string Comment { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    public int ReceiverId { get; set; }
}
