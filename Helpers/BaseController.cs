using Microsoft.AspNetCore.Mvc;
using CampaignManagement.Helpers.Middlewares;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Helpers
{
    public class BaseController : Controller
    {
        protected (int? userId, int accessLevel) GetUserContext()
        {
            var authToken = Request.Cookies["cmAuthToken"];
            if (string.IsNullOrEmpty(authToken))
                return (null, 0);

            try
            {
                var tokenInfo = TokenHelper.DecryptToken(authToken);
                if (tokenInfo == null)
                    return (null, 0);

                return (tokenInfo.UserId, tokenInfo.AccessLevel);
            }
            catch
            {
                return (null, 0);
            }
        }

        public async Task SetPagePermissionsAsync(IPermissionHelper permissionHelper, string controllerName, string actionName)
        {
            var (userId, accessLevel) = GetUserContext();
            if (userId.HasValue && accessLevel > 0)
            {
                var permissions = await permissionHelper.GetAllPagePermissionsAsync(
                    userId.Value,
                    accessLevel,
                    controllerName,
                    actionName
                );
                ViewBag.PagePermissions = permissions;
            }
            else
            {
                // Default permissions if not logged in (shouldn't happen, but for safety)
                ViewBag.PagePermissions = new PagePermissionsDTO
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
