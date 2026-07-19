using Microsoft.EntityFrameworkCore;
using CampaignManagement.Models.Entity;

namespace CampaignManagement.Helpers.DbContexts
{
    public class CampaignDbContext : DbContext
    {
        public CampaignDbContext(DbContextOptions<CampaignDbContext> options) : base(options) { }

        // Define DbSets for tables
        public DbSet<mstUsers> mstUsers { get; set; } = null!;
        public DbSet<mstAccessLevel> mstAccessLevel { get; set; } = null!;
        public DbSet<mstCoreAdmin> mstCoreAdmin { get; set; } = null!;
        public DbSet<trnUserActivity> trnUserActivity { get; set; } = null!;
        public DbSet<trnUserOtps> trnUserOtps { get; set; } = null!;
        public DbSet<mstCampaign> mstCampaigns { get; set; } = null!;
        public DbSet<mstInfluencer> mstInfluencers { get; set; } = null!;
        public DbSet<mstStakeholder> mstStakeholders { get; set; } = null!;
        public DbSet<trnDeliverable> trnDeliverables { get; set; } = null!;
        public DbSet<trnExpense> trnExpenses { get; set; } = null!;
        public DbSet<trnCampaignReports> trnCampaignReports { get; set; } = null!;
        public DbSet<trnCampaignPartner> trnCampaignPartners { get; set; } = null!;
        public DbSet<mstCreatorCode> mstCreatorCodes { get; set; } = null!;
    }
}
