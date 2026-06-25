using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("mstAccessLevel")]
    public class mstAccessLevel
    {
        [Key]
        public int mstAccessLevelId { get; set; }
        public string name { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
        public int? createdBy { get; set; }
        public DateTime? created_at { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
