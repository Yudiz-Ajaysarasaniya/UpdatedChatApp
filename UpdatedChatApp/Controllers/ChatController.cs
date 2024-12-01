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
        private readonly IWebHostEnvironment environment;

        public ChatController(IChatService chatService, IWebHostEnvironment environment)
        {
            this.chatService = chatService;
            this.environment = environment;
        }

        [HttpGet("GetChatHistory")]
        public async Task<IActionResult> GetChatHistory(Guid user1Id, Guid user2Id)
        {
            var messages = await chatService.GetChatHistoryAsync(user1Id, user2Id);

            if (messages == null) return NotFound("No Messages");

            return Ok(messages);
        }
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string senderId, [FromForm] string receiverId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return file URL
                // Return file URL as JSON object
                var fileUrl = $"/uploads/{uniqueFileName}";
                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


    }
}
