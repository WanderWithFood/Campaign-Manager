using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("trnCampaignPartners")]
    public class trnCampaignPartner
    {
        [Key]
        public int trnCampaignPartnerId { get; set; }
        
        [Required]
        public int campaignId { get; set; }
        
        [Required]
        public int influencerId { get; set; }
        
        public bool isActive { get; set; } = true;
        public DateTime? created_at { get; set; } = DateTime.Now;
    }
}
