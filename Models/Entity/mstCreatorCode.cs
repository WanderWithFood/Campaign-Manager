using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("mstCreatorCodes")]
    public class mstCreatorCode
    {
        [Key]
        public int mstCreatorCodeId { get; set; }
        
        [Required]
        public string code { get; set; } = string.Empty;
        
        public int influencerId { get; set; }
        
        public int? campaignId { get; set; }
        
        public int totalUsages { get; set; } = 0;
        
        public decimal incentivePerUsage { get; set; } = 0.00m;
        
        public string status { get; set; } = "Active";
        
        public bool isActive { get; set; } = true;
        
        public DateTime? created_at { get; set; } = DateTime.Now;
    }
}
