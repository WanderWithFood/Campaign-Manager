using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("mstCoreAdmin")]
    public class mstCoreAdmin
    {
        [Key]
        public int mstCoreAdminId { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string? password { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
