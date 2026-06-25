using Microsoft.AspNetCore.Mvc;
using CampaignManagement.Helpers.Middlewares;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Helpers
{
    public static class ControllerHelpers
    {
        public static (int? userId, int accessLevel) GetUserContext(this Controller controller)
        {
            var authToken = controller.Request.Cookies["cmAuthToken"];
            if (string.IsNullOrEmpty(authToken))
                return (null, 0);

            try
            {
                var tokenInfo = TokenHelper.DecryptToken(authToken);
                return (tokenInfo.UserId, tokenInfo.AccessLevel);
            }
            catch
            {
                return (null, 0);
            }
        }

        /// <summary>
        /// Extension method to set page permissions in ViewBag for any Controller
        /// </summary>
        public static async Task SetPagePermissionsAsync(this Controller controller, IPermissionHelper permissionHelper, string controllerName, string actionName)
        {
            var (userId, accessLevel) = controller.GetUserContext();
            if (userId.HasValue && accessLevel > 0)
            {
                var permissions = await permissionHelper.GetAllPagePermissionsAsync(
                    userId.Value,
                    accessLevel,
                    controllerName,
                    actionName
                );
                controller.ViewBag.PagePermissions = permissions;
            }
            else
            {
                // Default permissions if not logged in (shouldn't happen, but for safety)
                controller.ViewBag.PagePermissions = new PagePermissionsDTO
                {
                    CanView = false,
                    CanViewOthers = false,
                    CanCreate = false,
                    CanEdit = false,
                    CanEditOthers = false,
                    CanDelete = false,
                    CanDeleteOthers = false,
                    CanViewAllRecords = false
                };
            }
        }
    }
}
