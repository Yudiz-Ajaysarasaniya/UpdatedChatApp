using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpdatedChatApp.Helper;
using UpdatedChatApp.Interfaces;

namespace UpdatedChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;
        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;

        }

        [HttpGet("GetChatHistory")]
        public async Task<IActionResult> GetChatHistory(Guid user1Id, Guid user2Id)
        {
            var messages = await chatService.GetChatHistoryAsync(user1Id, user2Id);

            if (messages == null) return NotFound("No Messages");

            return Ok(messages);
        }
    }
}
