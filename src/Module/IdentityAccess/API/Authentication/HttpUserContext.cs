using BuildingBlocks.Application;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace IdentityAccess.API.Authentication
{
    public sealed class HttpUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpUserContext(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated
        {
            get
            {
                return _httpContextAccessor
                    .HttpContext?
                    .User
                    .Identity?
                    .IsAuthenticated
                    ?? false;
            }
        }

        public Guid UserId
        {
            get
            {
                var userIdValue =
                    _httpContextAccessor
                        .HttpContext?
                        .User
                        .FindFirst(
                            ClaimTypes.NameIdentifier)?
                            .Value;

                if (string.IsNullOrWhiteSpace(userIdValue))
                {
                    throw new UnauthorizedAccessException(
                        "Current user is not authenticated.");
                }

                if (!Guid.TryParse(
                    userIdValue,
                    out var userId))
                {
                    throw new UnauthorizedAccessException(
                        "Current user identifier is invalid.");
                }

                return userId;
            }
        }
    }
}
