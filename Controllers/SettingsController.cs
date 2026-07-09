using System.Threading.Tasks;
using CampaignManagement.Helpers;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IPermissionHelper _permissionHelper;

        public SettingsController(ISettingsRepository settingsRepository, IPermissionHelper permissionHelper)
        {
            _settingsRepository = settingsRepository;
            _permissionHelper = permissionHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await SetPagePermissionsAsync(_permissionHelper, "Settings", "Index");

            var users = await _settingsRepository.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Save(mstUsers user, IFormFile? profileImage)
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            user.updatedBy = userId.Value;
            var response = await _settingsRepository.SaveOrUpdateUserAsync(user, profileImage);
            return RedirectToAction("Index");
        }
    }
}
