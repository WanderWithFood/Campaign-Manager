using System.Collections.Generic;
using System.Threading.Tasks;
using CampaignManagement.Models.Entity;

namespace CampaignManagement.Interfaces
{
    public interface ICreatorCodesRepository
    {
        Task<List<mstCreatorCode>> GetAllCreatorCodesAsync();
        Task<List<mstCreatorCode>> GetCreatorCodesByInfluencerAsync(int influencerId);
        Task<mstCreatorCode?> GetCreatorCodeByIdAsync(int id);
        Task<int> CreateCreatorCodeAsync(mstCreatorCode code);
        Task<bool> UpdateCreatorCodeUsagesAsync(int id, int usages);
        Task<bool> DeleteCreatorCodeAsync(int id);
    }
}
