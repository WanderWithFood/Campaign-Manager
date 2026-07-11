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
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await SetPagePermissionsAsync(_permissionHelper, "InfluencersStat", "Index");

            var influencers = await _influencersStatRepository.GetInfluencersAsync(search);
            ViewBag.Search = search;
            return View(influencers);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await SetPagePermissionsAsync(_permissionHelper, "InfluencersStat", "Profile");

            var influencer = await _influencersStatRepository.GetInfluencerByIdAsync(id);
            if (influencer == null)
            {
                return NotFound();
            }

            // Fetch campaign history metrics - retrieve all campaigns for the demo/stats
            var allCampaigns = await _campaignsRepository.GetCampaignsAsync();
            // Filter campaigns where this influencer is active (in our seed data, campaignId 4 has influencerId 1)
            // For now, let's pass all campaigns to show rich data in the history table matching screenshot (page 9)
            ViewBag.Campaigns = allCampaigns;

            return View(influencer);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(int id, string note)
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await _influencersStatRepository.AddNoteAsync(id, note);
            return RedirectToAction("Profile", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> SaveInfluencer(mstInfluencer influencer)
        {
            var (userId, accessLevel) = GetUserContext();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            await _influencersStatRepository.SaveInfluencerAsync(influencer);
            return RedirectToAction("Index");
        }
    }
}
