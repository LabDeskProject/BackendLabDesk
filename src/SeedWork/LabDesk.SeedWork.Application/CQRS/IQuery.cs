using LabDesk.SeedWork.Application.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Application.CQRS
{
    public interface IQuery<TRespone> : IRequest<Result<TRespone>> { }

    public interface IQueryHandler <in TQuery , TRespone>
        : IRequestHandler<TQuery , Result<TRespone>>
        where TQuery : IQuery<TRespone> { } 
}
