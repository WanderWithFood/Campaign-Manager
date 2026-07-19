using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("mstCampaigns")]
    public class mstCampaign
    {
        [Key]
        public int mstCampaignId { get; set; }
        
        [Required]
        public string name { get; set; } = string.Empty;
        
        public string? campaignType { get; set; }
        public string? campaignCycle { get; set; }
        
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? duration { get; set; }
        
        public decimal totalSpend { get; set; } = 0.00m;
        public decimal conversionRate { get; set; } = 0.00m;
        
        public int downloadsBefore { get; set; } = 0;
        public int downloadsAfter { get; set; } = 0;
        
        public string? objective { get; set; }
        public string? targetAudience { get; set; }
        
        public decimal budget { get; set; } = 0.00m;
        
        public string? creatorName { get; set; }
        public string? socialMediaPlatforms { get; set; }
        
        // Influencer link
        public int? influencerId { get; set; }
        
        // Budget breakdown: 3 types
        public decimal basePay { get; set; } = 0.00m;
        
        [Column("incentive")]
        public decimal incentiveAmount { get; set; } = 0.00m;
        public decimal budgetThreshold { get; set; } = 0.00m;
        public decimal allowance { get; set; } = 0.00m;
        
        // Campaign details
        public string? termsAndConditions { get; set; }
        public string? influencerTag { get; set; }
        public string? endReason { get; set; }
        public int? totalReach { get; set; } = 0;
        
        public string status { get; set; } = "Active";
        
        public bool isActive { get; set; } = true;
        
        public int? createdBy { get; set; }
        public DateTime? created_at { get; set; } = DateTime.Now;
        public int? updatedBy { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
