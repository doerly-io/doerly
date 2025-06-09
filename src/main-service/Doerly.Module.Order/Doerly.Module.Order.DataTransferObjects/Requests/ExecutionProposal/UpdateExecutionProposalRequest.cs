using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.DataTransferObjects.Requests;
public class UpdateExecutionProposalRequest
{
    [MaxLength(1000, ErrorMessage = "FieldIsTooLong")]
    public string Comment { get; set; }
}
