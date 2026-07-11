using System.Collections.Generic;

namespace CampaignManagement.Models.DTOs
{
    public class DashboardStatsDTO
    {
        public int activeCampaigns { get; set; }
        public int totalCampaigns { get; set; }
        public string totalReach { get; set; } = "0";
        public decimal totalSpend { get; set; }
        public string totalDownloads { get; set; } = "0";
        public decimal avgCostPerDownload { get; set; }
        public List<ActiveCampaignListItemDTO> activeCampaignsList { get; set; } = new();
        public List<DownloadsGrowthDataPoint> downloadsGrowth { get; set; } = new();
    }

    public class ActiveCampaignListItemDTO
    {
        public int campaignId { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int daysLeft { get; set; }
        public string reach { get; set; } = string.Empty;
        public double completionPercentage { get; set; }
    }

    public class DownloadsGrowthDataPoint
    {
        public string label { get; set; } = string.Empty;
        public int value { get; set; }
    }
}
