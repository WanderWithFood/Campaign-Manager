using CampaignManagement.Models.Entity;

namespace CampaignManagement.Interfaces
{
    public interface ICampaignsRepository
    {
        Task<List<mstCampaign>> GetAllCampaignsAsync();
        Task<List<mstCampaign>> GetCampaignsAsync();
        Task<mstCampaign?> GetCampaignByIdAsync(int campaignId);
        Task<int> CreateCampaignAsync(mstCampaign campaign);
        Task<bool> UpdateCampaignAsync(mstCampaign campaign);
        Task<bool> EndCampaignAsync(int campaignId, string? reason = null);

        Task<List<trnExpense>> GetExpensesByCampaignIdAsync(int campaignId);
        Task<int> AddExpenseAsync(trnExpense expense);

        Task<List<trnDeliverable>> GetDeliverablesByCampaignIdAsync(int campaignId);
        Task<bool> ToggleDeliverableAsync(int deliverableId, bool isCompleted);

        Task<List<mstStakeholder>> GetStakeholdersByCampaignIdAsync(int campaignId);
        Task<int> AddStakeholderAsync(mstStakeholder stakeholder);
        Task<bool> DeleteStakeholderAsync(int stakeholderId);
        
        Task<bool> UpdateTotalReachAsync(int campaignId, int totalReach);
        
        Task<List<mstInfluencer>> GetCampaignPartnersAsync(int campaignId);
        Task<bool> AddCampaignPartnerAsync(int campaignId, int influencerId);
        Task<bool> RemoveCampaignPartnerAsync(int campaignId, int influencerId);
    }
}
