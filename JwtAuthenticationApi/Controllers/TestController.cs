namespace JwtAuthenticationApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace JwtAuthenticationApi.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class TestController : ControllerBase
        {
            [HttpGet("public")]
            public IActionResult PublicEndpoint()
            {
                return Ok(new
                {
                    message = "This endpoint is public."
                });
            }

            [Authorize]
            [HttpGet("protected")]
            public IActionResult ProtectedEndpoint()
            {
                return Ok(new
                {
                    message = "You have successfully accessed a protected endpoint.",
                    username = User.Identity?.Name,
                    role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                });
            }
            [Authorize(Roles = "Admin")]
            [HttpGet("admin")]
            public IActionResult AdminEndpoint()
            {
                return Ok(new
                {
                    message = "You have successfully accessed the Admin-only endpoint.",
                    username = User.Identity?.Name,
                    role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                });
            }
        }
    }
}
