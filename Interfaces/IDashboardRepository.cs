using System.Threading.Tasks;
using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardStatsDTO> GetDashboardStatsAsync();
    }
}
