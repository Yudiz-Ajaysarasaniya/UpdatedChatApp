using Microsoft.AspNetCore.Mvc;
using UpdatedChatApp.Interfaces;
using UpdatedChatApp.model.Request.Account;
using UpdatedChatApp.notify;

namespace UpdatedChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMessages messages;
        private readonly IAccountService accountService;

        public AccountController(IMessages messages, IAccountService accountService)
        {
            this.messages = messages;
            this.accountService = accountService;
            // this.hubContext = hubContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest register)
        {
            try
            {
                var result = await accountService.RegisterUserAsync(register);

                if (result == null) return BadRequest("Something went wrong");

                // Store the user's email in session
                //HttpContext.Session.SetString("userEmail", register.Email);

                return Ok(new { Message = "OTP has been sent successfully to your email." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred: " + ex.Message });
            }
        }


        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtp otpVerify)
        {
            try
            {
                // Retrieve the user's email from the session
                /*var email = HttpContext.Session.GetString("userEmail");

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { Message = "Email not found in session" });
                }*/

                var isVerified = await accountService.VerifyOtpAsync(otpVerify);
                if (isVerified.IsSuccess)
                {
                    return Ok(StatusCodes.Status200OK);
                }

                return BadRequest(new { Message = "Invalid or expired OTP" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login.Email)) return BadRequest("Email cannot be null");
                if (string.IsNullOrWhiteSpace(login.Password)) return BadRequest("Password cannot be null");

                var result = await accountService.LoginAsync(login);

                if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

                return Ok(new
                {
                    result.IsSuccess,
                    result.Data
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
    }
}
