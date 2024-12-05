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
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files, [FromForm] string senderId, [FromForm] string receiverId)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return BadRequest("No files uploaded");

                var uploadedFiles = new List<object>();
                var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                foreach (var file in files)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileUrl = $"/uploads/{uniqueFileName}";
                    uploadedFiles.Add(new { FileUrl = fileUrl });
                }

                return Ok(uploadedFiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> MessageCountAsync(Guid senderId, Guid receiverId)
        {
            if(senderId == null && receiverId == null)
            {
                return BadRequest();
            }

            var count = await chatService.MessageCount(senderId, receiverId);

            if (count == 0)
            {
                return BadRequest();
            }

            return Ok(count);
        }


    }
}
