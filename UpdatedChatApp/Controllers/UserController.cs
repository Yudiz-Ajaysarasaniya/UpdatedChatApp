using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpdatedChatApp.Interfaces;

namespace UpdatedChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string sender)
        {
            var response = await userService.GetUsers(sender);

            if (response == null || !response.Any()) return NotFound("No Users Found");
            return Ok(response);
        }
    }
}
