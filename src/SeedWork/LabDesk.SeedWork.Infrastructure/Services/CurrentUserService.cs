using LabDesk.SeedWork.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LabDesk.SeedWork.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(userIdStr, out var userId) ? userId : null;
            }
        }

        public Guid? OrganizationId
        {
            get
            {
                var orgIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue("organization_id");
                return Guid.TryParse(orgIdStr, out var orgId) ? orgId : null;
            }
        }
        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);


    }
}
