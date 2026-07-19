using System.Collections.Generic;
using System.Threading.Tasks;
using CampaignManagement.Models.Entity;

namespace CampaignManagement.Interfaces
{
    public interface IInfluencersStatRepository
    {
        Task<List<mstInfluencer>> GetInfluencersAsync(string? search = null);
        Task<mstInfluencer?> GetInfluencerByIdAsync(int id);
        Task<mstInfluencer> SaveInfluencerAsync(mstInfluencer influencer);
        Task<bool> AddNoteAsync(int influencerId, string note);
        Task<int> GetCreatorCodeUsagesAsync(int influencerId);
    }
}
