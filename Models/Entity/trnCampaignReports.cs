namespace CampaignManagement.Models.Entity
{
    public class trnCampaignReports
    {
        public int trnCampaignReportId { get; set; }
        public int campaignId { get; set; }
        public int impressions { get; set; }
        public int clicks { get; set; }
        public int conversions { get; set; }
        public decimal revenue { get; set; }
        public string? reportType { get; set; } // Daily, Weekly, Monthly
        public DateTime reportDate { get; set; }
        public DateTime created_at { get; set; }
    }
}
