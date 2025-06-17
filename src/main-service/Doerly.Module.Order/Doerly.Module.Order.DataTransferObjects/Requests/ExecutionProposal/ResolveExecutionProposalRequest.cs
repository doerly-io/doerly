using System.ComponentModel.DataAnnotations;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects.Requests;
public class ResolveExecutionProposalRequest
{
    [Required(ErrorMessage = "FieldIsRequired")]
    public EExecutionProposalStatus Status { get; set; }
}
