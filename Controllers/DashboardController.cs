using System;
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
        public async Task<IActionResult> Index(int? days, DateTime? fromDate, DateTime? toDate)
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await SetPagePermissionsAsync(_permissionHelper, "Dashboard", "Index");

            // Determine date range from params
            DateTime? filterFrom = fromDate;
            DateTime? filterTo = toDate;

            if (days.HasValue && days.Value > 0)
            {
                filterTo = DateTime.Now;
                filterFrom = DateTime.Now.AddDays(-days.Value);
            }
            else if (!fromDate.HasValue && !toDate.HasValue)
            {
                // Default to last 30 days
                filterTo = DateTime.Now;
                filterFrom = DateTime.Now.AddDays(-30);
            }

            ViewBag.Days = days ?? 30;
            ViewBag.FromDate = filterFrom?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = filterTo?.ToString("yyyy-MM-dd");

            var stats = await _dashboardRepository.GetDashboardStatsAsync(filterFrom, filterTo);
            return View(stats);
        }
    }
}
