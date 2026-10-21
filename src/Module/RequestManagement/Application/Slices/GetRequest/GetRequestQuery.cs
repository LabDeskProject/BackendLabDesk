using MediatR;
using RequestManagement.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.GetRequest
{

    public sealed record GetRequestQuery(Guid RequestId) : IRequest<RequestListItem?>;
}
