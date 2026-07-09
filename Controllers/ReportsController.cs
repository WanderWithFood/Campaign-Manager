using System.Threading.Tasks;
using CampaignManagement.Helpers;
using CampaignManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IPermissionHelper _permissionHelper;

        public ReportsController(IReportsRepository reportsRepository, IPermissionHelper permissionHelper)
        {
            _reportsRepository = reportsRepository;
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

            await SetPagePermissionsAsync(_permissionHelper, "Reports", "Index");

            var response = await _reportsRepository.GetReportsSummaryAsync();
            return View(response);
        }
    }
}
