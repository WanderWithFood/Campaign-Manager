using CampaignManagement.Interfaces;
using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Helpers
{
    public static class PermissionFilterHelper
    {
        /// <summary>
        /// Gets the list of user IDs that the current user is allowed to view records for.
        /// Returns empty list if canViewAllRecords is true (meaning view all).
        /// </summary>
        public static async Task<List<int>> GetAllowedUserIdsAsync(
            IPermissionHelper permissionHelper,
            int userId,
            int accessLevelId,
            string controllerName,
            string actionName)
        {
            var permissions = await permissionHelper.GetAllPagePermissionsAsync(userId, accessLevelId, controllerName, actionName);
            return permissions.AllowedUserIds;
        }

        /// <summary>
        /// Gets all permissions for a page in a single call
        /// </summary>
        public static async Task<PagePermissionsDTO> GetPagePermissionsAsync(
            IPermissionHelper permissionHelper,
            int userId,
            int accessLevelId,
            string controllerName,
            string actionName)
        {
            return await permissionHelper.GetAllPagePermissionsAsync(userId, accessLevelId, controllerName, actionName);
        }
    }
}
