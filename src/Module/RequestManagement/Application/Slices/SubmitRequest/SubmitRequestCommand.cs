using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.SubmitRequest
{
    public sealed record SubmitRequestCommand(Guid OrganizationId, Guid ServiceId, string Title, string? Description, string FormDataJson) : IRequest<Guid>;
}
