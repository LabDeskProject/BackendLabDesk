using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.Application.Slices.DecideApproval
{
    public sealed record DecideApprovalCommand(Guid WorkflowId, ApprovalDecision Decision, string? Comment) : IRequest;
}
