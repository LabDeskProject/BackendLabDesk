using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Application.CQRS
{
    public interface IQuery<TResult> : IRequest<TResult>
    {
    }
}
