using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("trnDeliverables")]
    public class trnDeliverable
    {
        [Key]
        public int trnDeliverableId { get; set; }
        
        [Required]
        public int campaignId { get; set; }
        
        public int? influencerId { get; set; }
        
        [Required]
        public string deliverable { get; set; } = string.Empty;
        
        public DateTime? completedOn { get; set; }
        public DateTime? deadline { get; set; }
        
        public bool isCompleted { get; set; } = false;
        
        public string? evidence { get; set; }
        
        public bool isActive { get; set; } = true;
    }
}
