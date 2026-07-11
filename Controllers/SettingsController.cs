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
            try
            {
                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await SetPagePermissionsAsync(_permissionHelper, "Settings", "Index");

                var users = await _settingsRepository.GetAllUsersAsync();
                return View(users ?? new List<mstUsers>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SettingsController.Index: {ex.Message}");
                ViewBag.Error = "Failed to load users. Please try again.";
                return View(new List<mstUsers>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(mstUsers user, IFormFile? profileImage)
        {
            try
            {
                if (user == null)
                {
                    ViewBag.Error = "Invalid user data";
                    return RedirectToAction("Index");
                }

                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "Please correct the errors in the form";
                    return RedirectToAction("Index");
                }

                // Validate file if provided
                if (profileImage != null)
                {
                    if (profileImage.Length > 5242880) // 5MB limit
                    {
                        ViewBag.Error = "Profile image size cannot exceed 5MB";
                        return RedirectToAction("Index");
                    }

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(profileImage.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension);
                    {
                        ViewBag.Error = "Invalid file format. Only JPG, PNG, and GIF are allowed.";
                        return RedirectToAction("Index");
                    }
                }

                user.updatedBy = userId.Value;
                var response = await _settingsRepository.SaveOrUpdateUserAsync(user, profileImage);
                
                if (response == null || !response.success)
                {
                    ViewBag.Error = response?.message ?? "Failed to save user";
                    return RedirectToAction("Index");
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SettingsController.Save: {ex.Message}");
                ViewBag.Error = "Failed to save user. Please try again.";
                return RedirectToAction("Index");
            }
        }
    }
}
