using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Interfaces;
using CampaignManagement.Models;
using CampaignManagement.Models.DTOs;
using CampaignManagement.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CampaignManagement.Repositories
{
    public class CampaignsRepository : ICampaignsRepository
    {
        private readonly CampaignDbContext _context;

        public CampaignsRepository(CampaignDbContext context)
        {
            _context = context;
        }

        public async Task<List<trnCampaignReports>> GetAllCampaignsAsync(int userId, int accessLevel)
        {
            try
            {
                var query = _context.trnCampaignReports.AsQueryable();

                // Apply permission filters based on access level
                if (accessLevel == 1) // Regular user - only their own campaigns
                {
                    query = query.Where(c => c.campaignId == userId);
                }
                else if (accessLevel == 2) // Manager - their own and team campaigns
                {
                    // Implement team logic as needed
                }
                // Access level 3+ has full access (admin)

                return await query.OrderByDescending(c => c.reportDate).ToListAsync();
            }
            catch (Exception)
            {
                return new List<trnCampaignReports>();
            }
        }

        public async Task<trnCampaignReports?> GetCampaignByIdAsync(int campaignId, int userId, int accessLevel)
        {
            try
            {
                var campaign = await _context.trnCampaignReports
                    .FirstOrDefaultAsync(c => c.trnCampaignReportId == campaignId);

                if (campaign == null)
                    return null;

                // Check permissions
                if (accessLevel == 1 && campaign.campaignId != userId)
                    return null;

                return campaign;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApiResponseDTO> CreateCampaignAsync(trnCampaignReports campaign, int userId)
        {
            try
            {
                campaign.campaignId = userId;
                campaign.created_at = DateTime.Now;

                _context.trnCampaignReports.Add(campaign);
                await _context.SaveChangesAsync();

                return ApiResponseDTO.SuccessResponse("Campaign created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseDTO.FailureResponse("An error occurred while creating the campaign.");
            }
        }

        public async Task<ApiResponseDTO> UpdateCampaignAsync(trnCampaignReports campaign, int userId, int accessLevel)
        {
            try
            {
                var existingCampaign = await _context.trnCampaignReports
                    .FirstOrDefaultAsync(c => c.trnCampaignReportId == campaign.trnCampaignReportId);

                if (existingCampaign == null)
                    return ApiResponseDTO.FailureResponse("Campaign not found.", 404);

                // Check permissions
                if (accessLevel == 1 && existingCampaign.campaignId != userId)
                    return ApiResponseDTO.FailureResponse("You don't have permission to update this campaign.", 403);

                existingCampaign.impressions = campaign.impressions;
                existingCampaign.clicks = campaign.clicks;
                existingCampaign.conversions = campaign.conversions;
                existingCampaign.revenue = campaign.revenue;
                existingCampaign.reportType = campaign.reportType;
                existingCampaign.reportDate = campaign.reportDate;

                await _context.SaveChangesAsync();

                return ApiResponseDTO.SuccessResponse("Campaign updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseDTO.FailureResponse("An error occurred while updating the campaign.");
            }
        }

        public async Task<ApiResponseDTO> DeleteCampaignAsync(int campaignId, int userId, int accessLevel)
        {
            try
            {
                var campaign = await _context.trnCampaignReports
                    .FirstOrDefaultAsync(c => c.trnCampaignReportId == campaignId);

                if (campaign == null)
                    return ApiResponseDTO.FailureResponse("Campaign not found.", 404);

                // Check permissions
                if (accessLevel == 1 && campaign.campaignId != userId)
                    return ApiResponseDTO.FailureResponse("You don't have permission to delete this campaign.", 403);

                _context.trnCampaignReports.Remove(campaign);
                await _context.SaveChangesAsync();

                return ApiResponseDTO.SuccessResponse("Campaign deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseDTO.FailureResponse("An error occurred while deleting the campaign.");
            }
        }

        public async Task<dynamic?> GetCampaignStatsAsync(int campaignId, int userId, int accessLevel)
        {
            try
            {
                var campaign = await _context.trnCampaignReports
                    .FirstOrDefaultAsync(c => c.trnCampaignReportId == campaignId);

                if (campaign == null)
                    return null;

                // Check permissions
                if (accessLevel == 1 && campaign.campaignId != userId)
                    return null;

                return new
                {
                    campaignId = campaign.trnCampaignReportId,
                    impressions = campaign.impressions,
                    clicks = campaign.clicks,
                    conversions = campaign.conversions,
                    revenue = campaign.revenue,
                    ctr = campaign.clicks > 0 ? ((decimal)campaign.clicks / campaign.impressions * 100).ToString("F2") + "%" : "0%",
                    conversionRate = campaign.conversions > 0 ? ((decimal)campaign.conversions / campaign.clicks * 100).ToString("F2") + "%" : "0%",
                    rpc = campaign.conversions > 0 ? (campaign.revenue / campaign.conversions).ToString("F2") : "0.00",
                    reportType = campaign.reportType,
                    reportDate = campaign.reportDate
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
