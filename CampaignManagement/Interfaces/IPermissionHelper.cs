using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Interfaces
{
    public interface IPermissionHelper
    {
        Task<bool> CanViewPageAsync(int userId, int accessLevelId, string controllerName, string actionName);
        Task<bool> CanCreateAsync(int userId, int accessLevelId, string controllerName, string actionName);
        Task<bool> CanEditRecordAsync(int userId, int accessLevelId, int recordCreatedBy, string controllerName, string actionName);
        Task<bool> CanDeleteRecordAsync(int userId, int accessLevelId, int recordCreatedBy, string controllerName, string actionName);
        Task<PagePermissionsDTO> GetAllPagePermissionsAsync(int userId, int accessLevelId, string controllerName, string actionName);
    }
}
