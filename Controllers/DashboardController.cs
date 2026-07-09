using System.Threading.Tasks;
using CampaignManagement.Helpers;
using CampaignManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IPermissionHelper _permissionHelper;

        public DashboardController(IDashboardRepository dashboardRepository, IPermissionHelper permissionHelper)
        {
            _dashboardRepository = dashboardRepository;
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

            await SetPagePermissionsAsync(_permissionHelper, "Dashboard", "Index");
            
            var stats = await _dashboardRepository.GetDashboardStatsAsync();
            return View(stats);
        }
    }
}
