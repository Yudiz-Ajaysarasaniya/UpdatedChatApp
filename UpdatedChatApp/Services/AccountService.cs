using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UpdatedChatApp.AppContext;
using UpdatedChatApp.Interfaces;
using UpdatedChatApp.model.Entities;
using UpdatedChatApp.model.Request.Account;
using UpdatedChatApp.model.Response.Authentication;
using UpdatedChatApp.model.Response.Base;
using UpdatedChatApp.notify;

namespace UpdatedChatApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext dbcontext;
        private readonly IMessages messages;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountService(AppDbContext chatAppContext, IMessages messages, IHttpContextAccessor httpContextAccessor)
        {
            this.dbcontext = chatAppContext;
            this.messages = messages;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await dbcontext.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
            if (user == null || user.Password != request.Password)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password",
                };
            }

            var token = GenerateToken(user);
            return new LoginResponse
            {
                IsSuccess = true,
                Data = new LoginResponseData
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = token
                }
            };
        }

        public string GenerateToken(Register register)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes("GSAJ0wPQz0aKvVsjbeW0a43a6yxtejmZ");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, register.Id.ToString()),
                    new Claim(ClaimTypes.Email, register.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }

        public async Task<string> RegisterUserAsync(RegisterRequest register)
        {
            if (string.IsNullOrEmpty(register.Email))
                throw new ArgumentException("Email is required");

            // Generate OTP and set expiry
            string otp = new Random().Next(100000, 999999).ToString();  // 6-digit OTP
            DateTime expiry = DateTime.UtcNow.AddMinutes(10);           // OTP valid for 10 minutes

            // Check if user already exists
            var user = dbcontext.Users.FirstOrDefault(u => u.Email == register.Email);
            if (user != null)
            {
                // If the user exists but hasn't been verified yet, update their OTP
                if (string.IsNullOrEmpty(user.OtpCode))
                    throw new ArgumentException("User already registered.");

                user.OtpCode = otp;
                user.OtpExpiry = expiry;
            }
            else
            {
                // New user, save their details
                var newUser = new Register
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    Password = register.Password,  // In real scenario, hash the password before saving
                    OtpCode = otp,
                    OtpExpiry = expiry
                };
                dbcontext.Users.Add(newUser);
            }
            await dbcontext.SaveChangesAsync();

            // Send OTP via email
            var subject = "Verify Your Email";
            var body = $"Your OTP code is {otp}. It is valid for 10 minutes.";
            await messages.SendMail(register.Email, subject, body);

            return "OTP sent successfully to your email";
        }


        public async Task<BaseResponse> VerifyOtpAsync(VerifyOtp otpVerify)
        {
            var user = await dbcontext.Users.FirstOrDefaultAsync(u => u.Email == otpVerify.Email);
            if (user == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found."
                };
            }

            // Check if the OTP matches and is still valid
            if (user.OtpCode == otpVerify.OtpCode && DateTime.UtcNow <= user.OtpExpiry)
            {
                // Invalidate OTP after successful verification
                //user.OtpCode = null;
                //user.OtpExpiry = null;
                //await dbcontext.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "OTP verification successful."
                };
            }

            return new BaseResponse
            {
                IsSuccess = false,
                ErrorMessage = "Invalid or expired OTP."
            };
        }

        public async Task<BaseResponse> ForgotPasswordAsync(ForgotPassword request)
        {
            var user = await dbcontext.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));

            if (user == null)
            {

                return new BaseResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found"
                };
            }

            // Generate OTP and set expiry
            string otp = new Random().Next(100000, 999999).ToString();  // 6-digit OTP
            DateTime expiry = DateTime.UtcNow.AddMinutes(10);

            user.OtpCode = otp;
            user.OtpExpiry = expiry;
            dbcontext.Users.Update(user);
            await dbcontext.SaveChangesAsync();

            // Send OTP via email
            var subject = "Verify Your Email";
            var body = $"Your OTP code is {otp}. It is valid for 10 minutes.";
            await messages.SendMail(request.Email, subject, body);

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "OTP sent successfully to your email"
            };
        }

        public async Task<BaseResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await dbcontext.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));

            if (user == null)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found"
                };
            }

            user.Password = request.Password;
            dbcontext.Users.Update(user);
            await dbcontext.SaveChangesAsync();


            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Password reset successfully"
            };
        }
    }
}
