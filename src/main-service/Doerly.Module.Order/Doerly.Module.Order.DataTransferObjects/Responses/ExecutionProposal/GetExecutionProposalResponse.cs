﻿using Doerly.Module.Order.Enums;
using Doerly.Module.Profile.DataTransferObjects.Profile;

namespace Doerly.Module.Order.DataTransferObjects.Responses;
public class GetExecutionProposalResponse
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string Comment { get; set; }

    public int SenderId { get; set; }

    public ProfileInfo Sender { get; set; }

    public int ReceiverId { get; set; }

    public ProfileInfo Receiver { get; set; }

    public EExecutionProposalStatus Status { get; set; }

    public DateTime DateCreated { get; set; }
}
