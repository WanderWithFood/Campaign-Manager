using System.Threading.Tasks;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        public Task<ApiResponseDTO> GetReportsSummaryAsync()
        {
            return Task.FromResult(new ApiResponseDTO
            {
                success = true,
                message = "Reports summary fetched"
            });
        }
    }
}
