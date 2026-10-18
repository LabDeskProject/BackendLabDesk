using APICore.Contracts.Team;
using ApplicationCore.Teams.CreateTeam;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace APICore.Controllers
{
    [ApiController]
    [Route("api/teams")]
    public sealed class TeamsController : ControllerBase
    {
        private readonly ISender _sender;

        public TeamsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            [FromBody] CreateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateTeamCommand(
                request.OrganizationId,
                request.Name,
                request.Description);

            var result = await _sender.Send(
                command,
                cancellationToken);

            return Created(
                $"/api/teams/{result.TeamId}",
                result);
        }
    }
}
