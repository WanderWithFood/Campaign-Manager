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
            try
            {
                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await SetPagePermissionsAsync(_permissionHelper, "Campaigns", "Index");

                ViewBag.ActiveStatus = status;
                var campaigns = await _repository.GetAllCampaignsAsync();
                return View(campaigns ?? new List<mstCampaign>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.Index: {ex.Message}");
                ViewBag.Error = "Failed to load campaigns. Please try again.";
                return View(new List<mstCampaign>());
            }
        }
        #endregion

        #region Campaign Details (Overview / Budget / Partners / Stakeholders)
        [HttpGet]
        public async Task<IActionResult> Details(int id, string tab = "Overview")
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid campaign ID");
                }

                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await SetPagePermissionsAsync(_permissionHelper, "Campaigns", "Details");

                var campaign = await _repository.GetCampaignByIdAsync(id);
                if (campaign == null)
                {
                    return NotFound("Campaign not found");
                }

                ViewBag.ActiveTab = tab;
                ViewBag.Expenses = await _repository.GetExpensesByCampaignIdAsync(id) ?? new List<trnExpense>();
                ViewBag.Deliverables = await _repository.GetDeliverablesByCampaignIdAsync(id) ?? new List<trnDeliverable>();
                ViewBag.Stakeholders = await _repository.GetStakeholdersByCampaignIdAsync(id) ?? new List<mstStakeholder>();

                return View(campaign);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.Details: {ex.Message}");
                ViewBag.Error = "Failed to load campaign details. Please try again.";
                return View();
            }
        }
        #endregion

        #region Create / Edit / End Campaign
        [HttpPost]
        public async Task<IActionResult> Create(mstCampaign campaign)
        {
            try
            {
                if (campaign == null)
                {
                    ViewBag.Error = "Invalid campaign data";
                    return View();
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Error = "Please correct the errors in the form";
                    return RedirectToAction("Index");
                }

                var newId = await _repository.CreateCampaignAsync(campaign);
                if (newId <= 0)
                {
                    ViewBag.Error = "Failed to create campaign";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Details", new { id = newId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.Create: {ex.Message}");
                ViewBag.Error = "Failed to create campaign. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(mstCampaign campaign)
        {
            try
            {
                if (campaign == null || campaign.mstCampaignId <= 0)
                {
                    ViewBag.Error = "Invalid campaign data";
                    return RedirectToAction("Index");
                }

                var result = await _repository.UpdateCampaignAsync(campaign);
                if (!result)
                {
                    ViewBag.Error = "Failed to update campaign";
                    return RedirectToAction("Details", new { id = campaign.mstCampaignId });
                }
                return RedirectToAction("Details", new { id = campaign.mstCampaignId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.Edit: {ex.Message}");
                ViewBag.Error = "Failed to update campaign. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EndCampaign(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid campaign ID");
                }

                var result = await _repository.EndCampaignAsync(id);
                if (!result)
                {
                    ViewBag.Error = "Failed to end campaign";
                    return RedirectToAction("Details", new { id });
                }
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.EndCampaign: {ex.Message}");
                ViewBag.Error = "Failed to end campaign. Please try again.";
                return RedirectToAction("Details", new { id });
            }
        }
        #endregion

        #region Deliverables
        [HttpPost]
        public async Task<IActionResult> ToggleDeliverable(int id, int campaignId, bool isCompleted = false)
        {
            try
            {
                if (id <= 0 || campaignId <= 0)
                {
                    return BadRequest("Invalid deliverable or campaign ID");
                }

                var result = await _repository.ToggleDeliverableAsync(id, isCompleted);
                if (!result)
                {
                    ViewBag.Error = "Failed to toggle deliverable";
                    return RedirectToAction("Details", new { id = campaignId, tab = "Partners" });
                }
                return RedirectToAction("Details", new { id = campaignId, tab = "Partners" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.ToggleDeliverable: {ex.Message}");
                ViewBag.Error = "Failed to toggle deliverable. Please try again.";
                return RedirectToAction("Details", new { id = campaignId, tab = "Partners" });
            }
        }
        #endregion

        #region Expenses
        [HttpPost]
        public async Task<IActionResult> SaveExpense(trnExpense expense)
        {
            try
            {
                if (expense == null || expense.campaignId <= 0)
                {
                    ViewBag.Error = "Invalid expense data";
                    return RedirectToAction("Index");
                }

                var result = await _repository.AddExpenseAsync(expense);
                if (result <= 0)
                {
                    ViewBag.Error = "Failed to save expense";
                    return RedirectToAction("Details", new { id = expense.campaignId, tab = "Budget" });
                }
                return RedirectToAction("Details", new { id = expense.campaignId, tab = "Budget" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.SaveExpense: {ex.Message}");
                ViewBag.Error = "Failed to save expense. Please try again.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Stakeholders
        [HttpPost]
        public async Task<IActionResult> SaveStakeholder(mstStakeholder stakeholder)
        {
            try
            {
                if (stakeholder == null)
                {
                    ViewBag.Error = "Invalid stakeholder data";
                    return RedirectToAction("Index");
                }

                var campaignId = stakeholder.campaignId ?? 0;
                if (campaignId <= 0)
                {
                    ViewBag.Error = "Invalid campaign ID";
                    return RedirectToAction("Index");
                }

                var result = await _repository.AddStakeholderAsync(stakeholder);
                if (result <= 0)
                {
                    ViewBag.Error = "Failed to save stakeholder";
                    return RedirectToAction("Details", new { id = campaignId, tab = "StakeholdersInfo" });
                }
                return RedirectToAction("Details", new { id = campaignId, tab = "StakeholdersInfo" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.SaveStakeholder: {ex.Message}");
                ViewBag.Error = "Failed to save stakeholder. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStakeholder(int id, int campaignId)
        {
            try
            {
                if (id <= 0 || campaignId <= 0)
                {
                    return BadRequest("Invalid stakeholder or campaign ID");
                }

                var result = await _repository.DeleteStakeholderAsync(id);
                if (!result)
                {
                    ViewBag.Error = "Failed to delete stakeholder";
                    return RedirectToAction("Details", new { id = campaignId, tab = "StakeholdersInfo" });
                }
                return RedirectToAction("Details", new { id = campaignId, tab = "StakeholdersInfo" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CampaignsController.DeleteStakeholder: {ex.Message}");
                ViewBag.Error = "Failed to delete stakeholder. Please try again.";
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}
