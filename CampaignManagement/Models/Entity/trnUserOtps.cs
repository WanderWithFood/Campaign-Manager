using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("trnUserOtps")]
    public class trnUserOtps
    {
        [Key]
        public int trnUserOtpId { get; set; }
        public int userId { get; set; }
        public string otp { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public DateTime expiresAt { get; set; }
        public bool isUsed { get; set; } = false;
    }
}
