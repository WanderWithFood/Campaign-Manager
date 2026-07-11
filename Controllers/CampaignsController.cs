using CampaignManagement.Helpers;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    public class CampaignsController : BaseController
    {
        private readonly ICampaignsRepository _repository;
        private readonly IPermissionHelper _permissionHelper;

        public CampaignsController(
            ICampaignsRepository repository,
            IPermissionHelper permissionHelper)
        {
            _repository = repository;
            _permissionHelper = permissionHelper;
        }

        #region Campaigns List
        [HttpGet]
        public async Task<IActionResult> Index(string status = "Active")
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await SetPagePermissionsAsync(_permissionHelper, "Campaigns", "Index");

            ViewBag.ActiveStatus = status;
            var campaigns = await _repository.GetAllCampaignsAsync();
            return View(campaigns);
        }
        #endregion

        #region Campaign Details (Overview / Budget / Partners / Stakeholders)
        [HttpGet]
        public async Task<IActionResult> Details(int id, string tab = "Overview")
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await SetPagePermissionsAsync(_permissionHelper, "Campaigns", "Details");

            var campaign = await _repository.GetCampaignByIdAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }

            ViewBag.ActiveTab = tab;
            ViewBag.Expenses = await _repository.GetExpensesByCampaignIdAsync(id);
            ViewBag.Deliverables = await _repository.GetDeliverablesByCampaignIdAsync(id);
            ViewBag.Stakeholders = await _repository.GetStakeholdersByCampaignIdAsync(id);

            return View(campaign);
        }
        #endregion

        #region Create / Edit / End Campaign
        [HttpPost]
        public async Task<IActionResult> Create(mstCampaign campaign)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var newId = await _repository.CreateCampaignAsync(campaign);
            return RedirectToAction("Details", new { id = newId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(mstCampaign campaign)
        {
            await _repository.UpdateCampaignAsync(campaign);
            return RedirectToAction("Details", new { id = campaign.mstCampaignId });
        }

        [HttpPost]
        public async Task<IActionResult> EndCampaign(int id)
        {
            await _repository.EndCampaignAsync(id);
            return RedirectToAction("Details", new { id });
        }
        #endregion

        #region Deliverables
        [HttpPost]
        public async Task<IActionResult> ToggleDeliverable(int id, int campaignId, bool isCompleted = false)
        {
            await _repository.ToggleDeliverableAsync(id, isCompleted);
            return RedirectToAction("Details", new { id = campaignId, tab = "Partners" });
        }
        #endregion

        #region Expenses
        [HttpPost]
        public async Task<IActionResult> SaveExpense(trnExpense expense)
        {
            await _repository.AddExpenseAsync(expense);
            return RedirectToAction("Details", new { id = expense.campaignId, tab = "Budget" });
        }
        #endregion

        #region Stakeholders
        [HttpPost]
        public async Task<IActionResult> SaveStakeholder(mstStakeholder stakeholder)
        {
            var campaignId = stakeholder.campaignId ?? 0;
            await _repository.AddStakeholderAsync(stakeholder);
            return RedirectToAction("Details", new { id = campaignId, tab = "StakeholdersInfo" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStakeholder(int id, int campaignId)
        {
            await _repository.DeleteStakeholderAsync(id);
            return RedirectToAction("Details", new { id = campaignId, tab = "StakeholdersInfo" });
        }
        #endregion
    }
}