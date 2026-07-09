using System.Collections.Generic;
using System.Threading.Tasks;
using CampaignManagement.Models.Entity;
using CampaignManagement.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace CampaignManagement.Interfaces
{
    public interface ISettingsRepository
    {
        #region User - CRUD Operations
        Task<List<mstUsers>> GetAllUsersAsync();
        Task<mstUsers?> GetUserByIdAsync(int id);
        Task<ApiResponseDTO> SaveOrUpdateUserAsync(mstUsers user, IFormFile? profileImageFile);
        #endregion
    }
}
