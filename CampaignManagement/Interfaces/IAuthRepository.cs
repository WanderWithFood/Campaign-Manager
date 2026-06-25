using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Interfaces
{
    public interface IAuthRepository
    {
        #region Login Page Interface Implementations
        Task<ApiResponseDTO> LoginAsync(string username, string password);
        #endregion

        #region OTP Page Interface Implementations
        string GenerateOTP();
        Task SaveOTPAsync(int userId, string otp);
        Task<bool> VerifyOTPAsync(int userId, string otp);
        Task<bool> SendOTPAsync(string email, string otp);
        #endregion

        #region Register Page Interface Implementations
        Task<ApiResponseDTO> RegisterAsync(RegisterDTO registerDTO);
        #endregion

        Task<ApiResponseDTO> CheckEmailExistsAsync(string email);
    }
}
