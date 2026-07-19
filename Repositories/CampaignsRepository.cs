using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CampaignManagement.Repositories
{
    public class CampaignsRepository : ICampaignsRepository
    {
        #region DB Context Reference
        private readonly CampaignDbContext _context;

        public CampaignsRepository(CampaignDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Campaign - CRUD Operations
        public Task<List<mstCampaign>> GetCampaignsAsync()
        {
            return GetAllCampaignsAsync();
        }

        public async Task<List<mstCampaign>> GetAllCampaignsAsync()
        {
            return await _context.mstCampaigns
                .Where(c => c.isActive)
                .OrderByDescending(c => c.created_at)
                .ToListAsync();
        }

        public async Task<mstCampaign?> GetCampaignByIdAsync(int campaignId)
        {
            return await _context.mstCampaigns
                .FirstOrDefaultAsync(c => c.mstCampaignId == campaignId && c.isActive);
        }

        public async Task<int> CreateCampaignAsync(mstCampaign campaign)
        {
            campaign.created_at = DateTime.Now;
            campaign.isActive = true;
            if (string.IsNullOrWhiteSpace(campaign.status))
            {
                campaign.status = "Active";
            }

            await _context.mstCampaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
            return campaign.mstCampaignId;
        }

        public async Task<bool> UpdateCampaignAsync(mstCampaign campaign)
        {
            var existing = await _context.mstCampaigns
                .FirstOrDefaultAsync(c => c.mstCampaignId == campaign.mstCampaignId);

            if (existing == null)
            {
                return false;
            }

            existing.name = campaign.name;
            existing.campaignType = campaign.campaignType;
            existing.campaignCycle = campaign.campaignCycle;
            existing.startDate = campaign.startDate;
            existing.endDate = campaign.endDate;
            existing.duration = campaign.duration;
            existing.totalSpend = campaign.totalSpend;
            existing.conversionRate = campaign.conversionRate;
            existing.objective = campaign.objective;
            existing.targetAudience = campaign.targetAudience;
            existing.budget = campaign.budget;
            existing.creatorName = campaign.creatorName;
            existing.socialMediaPlatforms = campaign.socialMediaPlatforms;
            
            // New fields
            existing.influencerId = campaign.influencerId;
            existing.basePay = campaign.basePay;
            existing.incentiveAmount = campaign.incentiveAmount;
            existing.budgetThreshold = campaign.budgetThreshold;
            existing.allowance = campaign.allowance;
            existing.termsAndConditions = campaign.termsAndConditions;
            existing.influencerTag = campaign.influencerTag;
            existing.totalReach = campaign.totalReach;
            
            existing.updated_at = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EndCampaignAsync(int campaignId, string? reason = null)
        {
            var existing = await _context.mstCampaigns
                .FirstOrDefaultAsync(c => c.mstCampaignId == campaignId);

            if (existing == null)
            {
                return false;
            }

            existing.status = "Completed";
            existing.endReason = reason;
            existing.updated_at = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> UpdateTotalReachAsync(int campaignId, int totalReach)
        {
            var existing = await _context.mstCampaigns
                .FirstOrDefaultAsync(c => c.mstCampaignId == campaignId);

            if (existing == null)
            {
                return false;
            }

            existing.totalReach = totalReach;
            existing.updated_at = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Expenses
        public async Task<List<trnExpense>> GetExpensesByCampaignIdAsync(int campaignId)
        {
            return await _context.trnExpenses
                .Where(e => e.campaignId == campaignId && e.isActive)
                .OrderByDescending(e => e.expenseDate)
                .ToListAsync();
        }

        public async Task<int> AddExpenseAsync(trnExpense expense)
        {
            expense.isActive = true;
            await _context.trnExpenses.AddAsync(expense);
            await _context.SaveChangesAsync();
            return expense.trnExpenseId;
        }
        #endregion

        #region Deliverables
        public async Task<List<trnDeliverable>> GetDeliverablesByCampaignIdAsync(int campaignId)
        {
            return await _context.trnDeliverables
                .Where(d => d.campaignId == campaignId && d.isActive)
                .OrderBy(d => d.deadline)
                .ToListAsync();
        }

        public async Task<bool> ToggleDeliverableAsync(int deliverableId, bool isCompleted)
        {
            var existing = await _context.trnDeliverables
                .FirstOrDefaultAsync(d => d.trnDeliverableId == deliverableId);

            if (existing == null)
            {
                return false;
            }

            existing.isCompleted = isCompleted;
            existing.completedOn = isCompleted ? DateTime.Now : null;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Stakeholders
        public async Task<List<mstStakeholder>> GetStakeholdersByCampaignIdAsync(int campaignId)
        {
            return await _context.mstStakeholders
                .Where(s => s.campaignId == campaignId && s.isActive)
                .OrderByDescending(s => s.created_at)
                .ToListAsync();
        }

        public async Task<int> AddStakeholderAsync(mstStakeholder stakeholder)
        {
            stakeholder.isActive = true;
            stakeholder.created_at = DateTime.Now;
            await _context.mstStakeholders.AddAsync(stakeholder);
            await _context.SaveChangesAsync();
            return stakeholder.mstStakeholderId;
        }

        public async Task<bool> DeleteStakeholderAsync(int stakeholderId)
        {
            var existing = await _context.mstStakeholders
                .FirstOrDefaultAsync(s => s.mstStakeholderId == stakeholderId);

            if (existing == null)
            {
                return false;
            }

            existing.isActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
