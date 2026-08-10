using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LabDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ← Require authentication
    public class ProtectedController : ControllerBase
    {
        /// <summary>
        /// Ví dụ endpoint protected - chỉ authenticated user mới truy cập
        /// </summary>
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Lấy thông tin từ JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var login = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                userId,
                login,
                email,
                role,
                message = "Đây là protected endpoint - chỉ authenticated user mới thấy"
            });
        }

        /// <summary>
        /// Endpoint chỉ Admin mới truy cập được
        /// </summary>
        [HttpGet("admin-data")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAdminData()
        {
            return Ok(new
            {
                message = "Chỉ Admin mới thấy data này"
            });
        }
    }
}
