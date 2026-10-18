using APICore.Contracts.Organizations;
using ApplicationCore.Organizations.CreateOrganization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace APICore.Controllers
{
    [ApiController]
    [Route("api/organizations")]
    [Authorize]
    public sealed class OrganizationsController : ControllerBase
    {
        private readonly ISender _sender;

        public OrganizationsController(
            ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(
            typeof(CreateOrganizationResponseDTO),
            StatusCodes.Status201Created)]
        [ProducesResponseType(
            typeof(ProblemDetails),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            typeof(ProblemDetails),
            StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(
            [FromBody] CreateOrganizationRequestDTO request,
            CancellationToken cancellationToken)
        {
            var command = new CreateOrganizationCommand(
                request.Name,
                request.Slug,
                request.Description);

            var result = await _sender.Send(
                command,
                cancellationToken);

            var response = new CreateOrganizationResponseDTO(
                result.OrganizationId,
                result.Name,
                result.Slug,
                result.Message);

            return Created(
                $"api/organizations/{result.OrganizationId}",
                response);
        }
    }
}
