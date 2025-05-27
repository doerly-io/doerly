using System.ComponentModel.DataAnnotations;

using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Dtos;
public class ResolveExecutionProposalRequest
{
    [Required(ErrorMessage = "FieldIsRequired")]
    public EExecutionProposalStatus Status { get; set; }
}
