using System;
using System.Collections.Generic;
using System.Text;
using LabDesk.SeedWork.Application.Results;
using MediatR;

namespace LabDesk.SeedWork.Application.Interfaces
{
    public interface ICommand : IRequest<Result> { }
    public interface ICommand<TRespone> : IRequest<Result<TRespone>> { }
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand { }
    public interface ICommandHandler<in TCommand, TRespone> 
        : IRequestHandler<TCommand , Result<TRespone>>
        where TCommand : ICommand<TRespone> { } 


}
