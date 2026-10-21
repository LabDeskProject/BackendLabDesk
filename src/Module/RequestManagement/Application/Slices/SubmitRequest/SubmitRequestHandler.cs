using MediatR;
using RequestManagement.Application.Interfaces;
using RequestManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.SubmitRequest
{
    public sealed class SubmitRequestHandler(IRequestRepository requests, IRequestNumberAllocator numbers, IRequestUserContext user, IRequestUnitOfWork unit, IRequestSubmissionTransaction transaction) : IRequestHandler<SubmitRequestCommand, Guid>
    {
        public async Task<Guid> Handle(SubmitRequestCommand command, CancellationToken ct)
        {
            if (!await requests.IsOrganizationMemberAsync(command.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User is not a member of this organization.");
            if (!await requests.ServiceIsActiveInOrganizationAsync(command.ServiceId, command.OrganizationId, ct)) 
                throw new InvalidOperationException("Service is not active for this organization.");
            ServiceRequest? entity = null;
            await transaction.ExecuteAsync(async token =>
            {
                var requestNumber = await numbers.NextAsync(command.OrganizationId, token);
                entity = ServiceRequest.Submit(command.OrganizationId, command.ServiceId, user.UserId, requestNumber, command.Title, command.Description, command.FormDataJson, DateTime.UtcNow);
                await requests.AddAsync(entity, token);
                await unit.SaveChangesAsync(token);
            }, ct);
            return entity!.Id;
        }
    }
}
