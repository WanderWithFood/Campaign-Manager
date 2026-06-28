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
    }
}
