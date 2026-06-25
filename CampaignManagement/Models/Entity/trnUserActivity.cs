using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("trnUserActivity")]
    public class trnUserActivity
    {
        [Key]
        public int trnUserActivityId { get; set; }
        public int userId { get; set; }
        public DateTime activityDateTime { get; set; }
        public string requestMethod { get; set; } = string.Empty;
        public string queryParams { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string pageUrl { get; set; } = string.Empty;
        public string remarks { get; set; } = string.Empty;
        public string functionName { get; set; } = string.Empty;
    }
}
