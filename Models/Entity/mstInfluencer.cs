using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("mstInfluencers")]
    public class mstInfluencer
    {
        [Key]
        public int mstInfluencerId { get; set; }
        
        [Required]
        public string name { get; set; } = string.Empty;
        
        public string? category { get; set; }
        public string? location { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string? gender { get; set; }
        public string? socialMediaPlatforms { get; set; }
        public string? creatorId { get; set; }
        
        public string? managerName { get; set; }
        public string? managerNumber { get; set; }
        
        public string? instagramProfile { get; set; }
        public string? whatsAppContact { get; set; }
        
        public string? integrationRequirements { get; set; }
        public string? exclusivityClause { get; set; }
        public string? contentDurationClause { get; set; }
        public string? paymentTerms { get; set; }
        
        public string? followers { get; set; }
        public decimal engagement { get; set; } = 0.00m;
        public string? avgViews { get; set; }
        public int estConversion { get; set; } = 0;
        public decimal estCostMin { get; set; } = 0.00m;
        public decimal estCostMax { get; set; } = 0.00m;
        
        public int reliabilityScore { get; set; } = 0;
        public string? notes { get; set; }
        
        public bool isActive { get; set; } = true;
        
        public DateTime? created_at { get; set; } = DateTime.Now;
    }
}
