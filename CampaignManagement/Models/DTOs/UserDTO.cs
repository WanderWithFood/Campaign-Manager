namespace CampaignManagement.Models.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthTokenPayload
    {
        public int UserId { get; set; }
        public string userName { get; set; }
        public int AccessLevel { get; set; }
    }

    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
    }

    public class UserDTO
    {
        public int mstUserId { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string password { get; set; }
        public int accessLevelId { get; set; }
        public int updatedBy { get; set; }
        public int risk_score { get; set; }
        public int streak_count { get; set; }
        public int longest_streak { get; set; }
        public DateTime? last_active_at { get; set; }
        public string user_status { get; set; }
        public string? user_tags { get; set; }
        public bool is_shadow_banned { get; set; }
        public string? notes { get; set; }
        public string? device_type { get; set; }
        public string? app_version { get; set; }
    }

    public class ProfileDTO
    {
        public int mstUserId { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int accessLevelId { get; set; }
        public int updatedBy { get; set; }
        public DateTime DOB { get; set; }
    }

    public class UserListDTO
    {
        public int mstUserId { get; set; }
        public string userName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string? accessLevelName { get; set; }
        public int? createdBy { get; set; }
        public string? gender { get; set; }
        public int age { get; set; }
        public DateTime? created_at { get; set; }
        public string? user_type { get; set; }
        public int risk_score { get; set; }
        public int streak_count { get; set; }
        public string? user_status { get; set; }
    }

    public class AccessLevelDTO
    {
        public int mstAccessLevelId { get; set; }
        public string name { get; set; }
        public int createdBy { get; set; }
    }

    public class SystemLogDTO
    {
        public int logId { get; set; }
        public string? userName { get; set; }
        public string? requestMethod { get; set; }
        public string? functionName { get; set; }
        public string? queryParams { get; set; }
        public string? pageUrl { get; set; }
        public string? remarks { get; set; }
        public string timestamp { get; set; }
    }

    public class SubAdminUserDTO
    {
        public int mstUserId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}
