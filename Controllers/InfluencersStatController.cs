using System.Threading.Tasks;
using CampaignManagement.Helpers;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    public class InfluencersStatController : BaseController
    {
        private readonly IInfluencersStatRepository _influencersStatRepository;
        private readonly ICampaignsRepository _campaignsRepository;
        private readonly IPermissionHelper _permissionHelper;

        public InfluencersStatController(
            IInfluencersStatRepository influencersStatRepository,
            ICampaignsRepository campaignsRepository,
            IPermissionHelper permissionHelper)
        {
            _influencersStatRepository = influencersStatRepository;
            _campaignsRepository = campaignsRepository;
            _permissionHelper = permissionHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            try
            {
                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await SetPagePermissionsAsync(_permissionHelper, "InfluencersStat", "Index");

                var influencers = await _influencersStatRepository.GetInfluencersAsync(search);
                ViewBag.Search = search;
                return View(influencers ?? new List<mstInfluencer>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InfluencersStatController.Index: {ex.Message}");
                ViewBag.Error = "Failed to load influencers. Please try again.";
                return View(new List<mstInfluencer>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid influencer ID");
                }

                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await SetPagePermissionsAsync(_permissionHelper, "InfluencersStat", "Profile");

                var influencer = await _influencersStatRepository.GetInfluencerByIdAsync(id);
                if (influencer == null)
                {
                    return NotFound("Influencer not found");
                }

                var allCampaigns = await _campaignsRepository.GetCampaignsAsync();
                // Filter to only show campaigns linked to this influencer
                var linkedCampaigns = allCampaigns?
                    .Where(c => c.influencerId == id || c.creatorName == influencer.name)
                    .ToList() ?? new List<mstCampaign>();
                ViewBag.Campaigns = linkedCampaigns;

                return View(influencer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InfluencersStatController.Profile: {ex.Message}");
                ViewBag.Error = "Failed to load influencer profile. Please try again.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(int id, string note)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid influencer ID");
                }

                if (string.IsNullOrWhiteSpace(note))
                {
                    ViewBag.Error = "Note cannot be empty";
                    return RedirectToAction("Profile", new { id });
                }

                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await _influencersStatRepository.AddNoteAsync(id, note);
                return RedirectToAction("Profile", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InfluencersStatController.AddNote: {ex.Message}");
                ViewBag.Error = "Failed to add note. Please try again.";
                return RedirectToAction("Profile", new { id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTotalReach(int campaignId, int totalReach, int influencerId)
        {
            try
            {
                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (campaignId > 0)
                {
                    await _campaignsRepository.UpdateTotalReachAsync(campaignId, totalReach);
                }
                return RedirectToAction("Profile", new { id = influencerId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InfluencersStatController.UpdateTotalReach: {ex.Message}");
                return RedirectToAction("Profile", new { id = influencerId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveInfluencer(mstInfluencer influencer)
        {
            try
            {
                if (influencer == null)
                {
                    ViewBag.Error = "Invalid influencer data";
                    return RedirectToAction("Index");
                }

                var (userId, accessLevel) = GetUserContext();
                if (!userId.HasValue)
                {
                    return RedirectToAction("Login", "Auth");
                }

                await _influencersStatRepository.SaveInfluencerAsync(influencer);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InfluencersStatController.SaveInfluencer: {ex.Message}");
                ViewBag.Error = "Failed to save influencer. Please try again.";
                return RedirectToAction("Index");
            }
        }
    }
}
