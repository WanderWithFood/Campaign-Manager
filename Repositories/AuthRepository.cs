using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Helpers.Middlewares;
using CampaignManagement.Interfaces;
using CampaignManagement.Models;
using CampaignManagement.Models.DTOs;
using CampaignManagement.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CampaignManagement.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CampaignDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(CampaignDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region Login Page Interface Implementations
        public async Task<ApiResponseDTO> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _context.mstUsers
                    .FirstOrDefaultAsync(u => u.email == username && u.password == password && u.isActive);

                if (user == null)
                {
                    return ApiResponseDTO.FailureResponse(clsResponseText.sInvalidUsernameUserpass);
                }

                var tokenPayload = new AuthTokenPayload
                {
                    UserId = user.mstUserId,
                    userName = user.name,
                    AccessLevel = user.userAccessLevel
                };

                var token = TokenHelper.EncryptToken(tokenPayload);

                return new ApiResponseDTO
                {
                    success = true,
                    message = clsResponseText.sLoginSuccessful,
                    data = new { token, userId = user.mstUserId, userName = user.name, accessLevel = user.userAccessLevel }
                };
            }
            catch (Exception)
            {
                return ApiResponseDTO.FailureResponse(clsResponseText.sSomethingWentWrong);
            }
        }
        #endregion

        #region OTP Page Interface Implementations
        public string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task SaveOTPAsync(int userId, string otp)
        {
            var otpRecord = new trnUserOtps
            {
                userId = userId,
                otp = otp,
                createdAt = DateTime.Now,
                expiresAt = DateTime.Now.AddMinutes(5),
                isUsed = false
            };

            _context.trnUserOtps.Add(otpRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyOTPAsync(int userId, string otp)
        {
            var otpRecord = await _context.trnUserOtps
                .Where(o => o.userId == userId && o.otp == otp && !o.isUsed && o.expiresAt > DateTime.Now)
                .OrderByDescending(o => o.createdAt)
                .FirstOrDefaultAsync();

            if (otpRecord == null) return false;

            otpRecord.isUsed = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendOTPAsync(string email, string otp)
        {
            // TODO: Implement email sending via Brevo or SMTP
            // For now, return true (OTP is saved to DB, can be verified manually)
            return true;
        }
        #endregion

        #region Register Page Interface Implementations
        public async Task<ApiResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var existingUser = await _context.mstUsers
                    .FirstOrDefaultAsync(u => u.email == registerDTO.Email);

                if (existingUser != null)
                {
                    return ApiResponseDTO.FailureResponse(clsResponseText.sEmailPhoneAlreadyExists);
                }

                var user = new mstUsers
                {
                    name = registerDTO.Name,
                    gender = registerDTO.Gender,
                    phoneNumber = registerDTO.PhoneNumber,
                    email = registerDTO.Email,
                    password = registerDTO.Password,
                    dateOfBirth = registerDTO.DOB,
                    userAccessLevel = 1, // Default user access level
                    isActive = true,
                    created_at = DateTime.Now
                };

                _context.mstUsers.Add(user);
                await _context.SaveChangesAsync();

                return ApiResponseDTO.SuccessResponse(clsResponseText.sSavedSuccessfully);
            }
            catch (Exception)
            {
                return ApiResponseDTO.FailureResponse(clsResponseText.sSomethingWentWrong);
            }
        }
        #endregion

        public async Task<ApiResponseDTO> CheckEmailExistsAsync(string email)
        {
            var exists = await _context.mstUsers.AnyAsync(u => u.email == email);
            return new ApiResponseDTO
            {
                success = true,
                data = new { exists }
            };
        }
    }
}
