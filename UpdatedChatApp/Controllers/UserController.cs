using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserByUserId(Guid id)
        {
            try
            {
                var user = await userService.GetUserByUserId(id);
                if (user == null)
                {
                    return NotFound($"User not found with ID: {id}");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving user: {ex.Message}");
            }
        }
    }
}
