using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace BuildingBlocks.Application.CQRS
{
    public interface ICommand<TResult> : IRequest<TResult>
    {
    }
}
