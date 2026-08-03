using LabDesk.Modules.Identity.Application.Commands.CreateOrganization;
using LabDesk.Modules.Identity.Application.Queries.GetOrganizationBySlug;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Presentation.Organization
{
    public class OrganizationController : ApiController
    {
        /// <summary>
        /// Tạo tổ chức / chi nhánh Lab mới
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrganization(
            [FromBody] CreateOrganizationCommand command,
            CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(
                nameof(Application.Queries.GetOrganizationBySlug),
                new { id = result.Value },
                result.Value);
        }

        /// <summary>
        /// Lấy thông tin tổ chức theo Slug
        /// </summary>
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrganizationBySlug(
            string slug,
            CancellationToken cancellationToken)
        {
            var query = new GetOrganizationBySlugQuery(slug);
            var result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
