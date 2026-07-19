using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampaignManagement.Helpers;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    public class CreatorCodesController : BaseController
    {
        private readonly ICreatorCodesRepository _repository;
        private readonly IInfluencersStatRepository _influencersRepository;
        private readonly ICampaignsRepository _campaignsRepository;
        private readonly IPermissionHelper _permissionHelper;

        public CreatorCodesController(
            ICreatorCodesRepository repository,
            IInfluencersStatRepository influencersRepository,
            ICampaignsRepository campaignsRepository,
            IPermissionHelper permissionHelper)
        {
            _repository = repository;
            _influencersRepository = influencersRepository;
            _campaignsRepository = campaignsRepository;
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

                await SetPagePermissionsAsync(_permissionHelper, "CreatorCodes", "Index");

                var creatorCodes = await _repository.GetAllCreatorCodesAsync();
                var influencers = await _influencersRepository.GetInfluencersAsync();
                var campaigns = await _campaignsRepository.GetCampaignsAsync();

                ViewBag.Influencers = influencers ?? new List<mstInfluencer>();
                ViewBag.Campaigns = campaigns ?? new List<mstCampaign>();

                return View(creatorCodes ?? new List<mstCreatorCode>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreatorCodesController.Index: {ex.Message}");
                ViewBag.Error = "Failed to load creator codes.";
                ViewBag.Influencers = new List<mstInfluencer>();
                ViewBag.Campaigns = new List<mstCampaign>();
                return View(new List<mstCreatorCode>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(mstCreatorCode creatorCode)
        {
            try
            {
                if (creatorCode == null || string.IsNullOrWhiteSpace(creatorCode.code))
                {
                    ViewBag.Error = "Invalid creator code data.";
                    return RedirectToAction("Index");
                }

                await _repository.CreateCreatorCodeAsync(creatorCode);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreatorCodesController.Create: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid creator code ID");
                }

                await _repository.DeleteCreatorCodeAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreatorCodesController.Delete: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsages(int id, int totalUsages)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid creator code ID");
                }

                await _repository.UpdateCreatorCodeUsagesAsync(id, totalUsages);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreatorCodesController.UpdateUsages: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
    }
}
