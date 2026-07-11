using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("mstStakeholders")]
    public class mstStakeholder
    {
        [Key]
        public int mstStakeholderId { get; set; }
        
        public int? campaignId { get; set; }
        
        [Required]
        public string name { get; set; } = string.Empty;
        
        public string? role { get; set; }
        public string? company { get; set; }
        public string? mobile { get; set; }
        public string? email { get; set; }
        public string? location { get; set; }
        
        public bool isActive { get; set; } = true;
        
        public DateTime? created_at { get; set; } = DateTime.Now;
    }
}
