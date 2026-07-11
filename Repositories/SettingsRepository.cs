using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Interfaces;
using CampaignManagement.Models;
using CampaignManagement.Models.DTOs;
using CampaignManagement.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CampaignManagement.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly CampaignDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IApiResponseRepository _apiResponseRepository;

        public SettingsRepository(
            CampaignDbContext context, 
            IConfiguration configuration,
            IApiResponseRepository apiResponseRepository)
        {
            _context = context;
            _configuration = configuration;
            _apiResponseRepository = apiResponseRepository;
        }

        #region User - CRUD Operations
        public async Task<List<mstUsers>> GetAllUsersAsync()
        {
            return await _context.mstUsers
                .Where(u => u.isActive)
                .OrderByDescending(u => u.created_at)
                .ToListAsync();
        }

        public async Task<mstUsers?> GetUserByIdAsync(int id)
        {
            return await _context.mstUsers
                .FirstOrDefaultAsync(u => u.mstUserId == id && u.isActive);
        }

        public async Task<ApiResponseDTO> SaveOrUpdateUserAsync(mstUsers user, IFormFile? profileImageFile)
        {
            try
            {
                if (user.mstUserId == 0)
                {
                    // Create new
                    var existing = await _context.mstUsers.AnyAsync(u => u.email == user.email && u.isActive);
                    if (existing)
                    {
                        return _apiResponseRepository.FailureResponse(new ApiResponseDTO
                        {
                            message = clsResponseText.sEmailPhoneAlreadyExists
                        });
                    }

                    user.created_at = DateTime.Now;
                    user.isActive = true;
                    _context.mstUsers.Add(user);
                }
                else
                {
                    // Update
                    var existingUser = await _context.mstUsers.FindAsync(user.mstUserId);
                    if (existingUser == null)
                    {
                        return _apiResponseRepository.FailureResponse(new ApiResponseDTO
                        {
                            message = clsResponseText.sRecordNotFound
                        });
                    }

                    existingUser.name = user.name;
                    existingUser.email = user.email;
                    existingUser.phoneNumber = user.phoneNumber;
                    existingUser.gender = user.gender;
                    existingUser.dateOfBirth = user.dateOfBirth;
                    if (!string.IsNullOrEmpty(user.password))
                    {
                        existingUser.password = user.password;
                    }
                    existingUser.userAccessLevel = user.userAccessLevel;
                    existingUser.updatedBy = user.updatedBy;
                    existingUser.updated_at = DateTime.Now;
                }

                if (profileImageFile != null && profileImageFile.Length > 0)
                {
                    var uploadFolder = _configuration["UploadSettings:CommonUploadFolder"] ?? "Uploads";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadFolder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fileName = $"{Guid.NewGuid()}_{profileImageFile.FileName}";
                    var filePath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImageFile.CopyToAsync(stream);
                    }

                    // Save file path
                    user.FirebaseId = Path.Combine(uploadFolder, fileName).Replace("\\", "/"); // Hack: reuse FirebaseId as avatar path
                }

                await _context.SaveChangesAsync();
                return _apiResponseRepository.SuccessResponse(new ApiResponseDTO
                {
                    message = clsResponseText.sSavedSuccessfully,
                    data = user
                });
            }
            catch (Exception ex)
            {
                return _apiResponseRepository.FailureResponse(new ApiResponseDTO
                {
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
