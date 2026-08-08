using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public class UnitOfWorkCommandHandlerDecorator<T> : IRequestHandler<T>
       where T : IRequest
    {
        private readonly IRequestHandler<T> _decorated;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommandHandlerDecorator(
            IRequestHandler<T> decorated,
            IUnitOfWork unitOfWork)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(T command, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(command, cancellationToken);

            await this._unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
