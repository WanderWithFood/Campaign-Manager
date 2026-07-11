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
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return ApiResponseDTO.FailureResponse("Username and password are required");
                }

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

                if (string.IsNullOrEmpty(token))
                {
                    return ApiResponseDTO.FailureResponse("Failed to generate authentication token");
                }

                return new ApiResponseDTO
                {
                    success = true,
                    message = clsResponseText.sLoginSuccessful,
                    data = new { token, userId = user.mstUserId, userName = user.name, accessLevel = user.userAccessLevel }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return ApiResponseDTO.FailureResponse(clsResponseText.sSomethingWentWrong);
            }
        }
        #endregion

        #region OTP Page Interface Implementations
        public string GenerateOTP()
        {
            try
            {
                Random random = new Random();
                return random.Next(100000, 999999).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OTP generation error: {ex.Message}");
                throw;
            }
        }

        public async Task SaveOTPAsync(int userId, string otp)
        {
            try
            {
                if (userId <= 0 || string.IsNullOrWhiteSpace(otp))
                {
                    throw new ArgumentException("Invalid userId or OTP");
                }

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
            catch (Exception ex)
            {
                Console.WriteLine($"OTP save error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> VerifyOTPAsync(int userId, string otp)
        {
            try
            {
                if (userId <= 0 || string.IsNullOrWhiteSpace(otp))
                {
                    return false;
                }

                var otpRecord = await _context.trnUserOtps
                    .Where(o => o.userId == userId && o.otp == otp && !o.isUsed && o.expiresAt > DateTime.Now)
                    .OrderByDescending(o => o.createdAt)
                    .FirstOrDefaultAsync();

                if (otpRecord == null) return false;

                otpRecord.isUsed = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OTP verification error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendOTPAsync(string email, string otp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
                {
                    return false;
                }

                // TODO: Implement email sending via Brevo or SMTP
                // For now, return true (OTP is saved to DB, can be verified manually)
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OTP send error: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Register Page Interface Implementations
        public async Task<ApiResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                if (registerDTO == null || string.IsNullOrWhiteSpace(registerDTO.Email) || string.IsNullOrWhiteSpace(registerDTO.Password))
                {
                    return ApiResponseDTO.FailureResponse("Email and password are required");
                }

                var existingUser = await _context.mstUsers
                    .FirstOrDefaultAsync(u => u.email == registerDTO.Email);

                if (existingUser != null)
                {
                    return ApiResponseDTO.FailureResponse(clsResponseText.sEmailPhoneAlreadyExists);
                }

                var user = new mstUsers
                {
                    name = registerDTO.Name ?? "User",
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
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return ApiResponseDTO.FailureResponse(clsResponseText.sSomethingWentWrong);
            }
        }
        #endregion

        public async Task<ApiResponseDTO> CheckEmailExistsAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return new ApiResponseDTO
                    {
                        success = false,
                        message = "Email is required",
                        data = new { exists = false }
                    };
                }

                var exists = await _context.mstUsers.AnyAsync(u => u.email == email);
                return new ApiResponseDTO
                {
                    success = true,
                    data = new { exists }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email check error: {ex.Message}");
                return new ApiResponseDTO
                {
                    success = false,
                    message = "Failed to check email",
                    data = new { exists = false }
                };
            }
        }
    }
}
