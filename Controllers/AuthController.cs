using Microsoft.AspNetCore.Mvc;

namespace dotnet_auth.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController: Controller
    {
        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("success");
        }
    }
}