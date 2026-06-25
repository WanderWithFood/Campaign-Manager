using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("mstUsers")]
    public class mstUsers
    {
        [Key]
        public int mstUserId { get; set; }
        public string name { get; set; } = string.Empty;
        public string? gender { get; set; }
        public string? phoneNumber { get; set; }
        public string email { get; set; } = string.Empty;
        public string? password { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string? FirebaseId { get; set; }
        public int userAccessLevel { get; set; }
        public bool isActive { get; set; } = true;
        public int? createdBy { get; set; }
        public DateTime? created_at { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updated_at { get; set; }
        public int risk_score { get; set; } = 0;
        public int streak_count { get; set; } = 0;
        public int longest_streak { get; set; } = 0;
        public DateTime? last_active_at { get; set; }
        public string? user_status { get; set; }
        public string? user_tags { get; set; }
        public bool is_shadow_banned { get; set; } = false;
        public string? notes { get; set; }
        public string? device_type { get; set; }
        public string? app_version { get; set; }
    }
}
