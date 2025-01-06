using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult GetUsername()
        
        {
            /*var windowsIdentity = WindowsIdentity.GetCurrent();
            var username = windowsIdentity.Name; // Get the full // Fetch Windows username (e.g., DOMAIN\username)*/
            var username = HttpContext.User.Identity?.Name ?? "Unknown User";
            return Ok(new { username });
        }


    }
}
