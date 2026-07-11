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
            try
            {
                var authToken = Request.Cookies["cmAuthToken"];
                if (string.IsNullOrEmpty(authToken))
                    return (null, 0);

                var tokenInfo = TokenHelper.DecryptToken(authToken);
                if (tokenInfo == null)
                    return (null, 0);

                return (tokenInfo.UserId, tokenInfo.AccessLevel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token decryption error in GetUserContext: {ex.Message}");
                return (null, 0);
            }
        }

        public async Task SetPagePermissionsAsync(IPermissionHelper permissionHelper, string controllerName, string actionName)
        {
            try
            {
                if (permissionHelper == null || string.IsNullOrWhiteSpace(controllerName) || string.IsNullOrWhiteSpace(actionName))
                {
                    SetDefaultPermissions();
                    return;
                }

                var (userId, accessLevel) = GetUserContext();
                if (userId.HasValue && accessLevel > 0)
                {
                    var permissions = await permissionHelper.GetAllPagePermissionsAsync(
                        userId.Value,
                        accessLevel,
                        controllerName,
                        actionName
                    );
                    ViewBag.PagePermissions = permissions ?? GetDefaultPermissions();
                }
                else
                {
                    SetDefaultPermissions();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetPagePermissionsAsync: {ex.Message}");
                SetDefaultPermissions();
            }
        }

        private void SetDefaultPermissions()
        {
            ViewBag.PagePermissions = GetDefaultPermissions();
        }

        private PagePermissionsDTO GetDefaultPermissions()
        {
            return new PagePermissionsDTO
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
