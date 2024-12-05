using UpdatedChatApp.model.Request.Account;
using UpdatedChatApp.model.Response.Authentication;
using UpdatedChatApp.model.Response.Base;

namespace UpdatedChatApp.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<string> RegisterUserAsync(RegisterRequest register);
        Task<BaseResponse> VerifyOtpAsync(VerifyOtp otpVerify);
        Task<BaseResponse> ForgotPasswordAsync(ForgotPassword request);
    }
}
