using ChatApp.model.Request.Account;
using ChatApp.model.Response.Authentication;
using ChatApp.model.Response.Base;

namespace UpdatedChatApp.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<string> RegisterUserAsync(RegisterRequest register);
        Task<BaseResponse> VerifyOtpAsync(VerifyOtp otpVerify);
    }
}
