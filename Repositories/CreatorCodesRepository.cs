using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace CampaignManagement.Repositories
{
    public class CreatorCodesRepository : ICreatorCodesRepository
    {
        private readonly CampaignDbContext _context;

        public CreatorCodesRepository(CampaignDbContext context)
        {
            _context = context;
        }

        public async Task<List<mstCreatorCode>> GetAllCreatorCodesAsync()
        {
            return await _context.mstCreatorCodes
                .Where(c => c.isActive)
                .OrderByDescending(c => c.created_at)
                .ToListAsync();
        }

        public async Task<List<mstCreatorCode>> GetCreatorCodesByInfluencerAsync(int influencerId)
        {
            return await _context.mstCreatorCodes
                .Where(c => c.influencerId == influencerId && c.isActive)
                .OrderByDescending(c => c.created_at)
                .ToListAsync();
        }

        public async Task<mstCreatorCode?> GetCreatorCodeByIdAsync(int id)
        {
            return await _context.mstCreatorCodes
                .FirstOrDefaultAsync(c => c.mstCreatorCodeId == id && c.isActive);
        }

        public async Task<int> CreateCreatorCodeAsync(mstCreatorCode code)
        {
            code.created_at = DateTime.Now;
            code.isActive = true;
            if (string.IsNullOrWhiteSpace(code.status))
            {
                code.status = "Active";
            }

            await _context.mstCreatorCodes.AddAsync(code);
            await _context.SaveChangesAsync();
            return code.mstCreatorCodeId;
        }

        public async Task<bool> UpdateCreatorCodeUsagesAsync(int id, int usages)
        {
            var existing = await _context.mstCreatorCodes.FindAsync(id);
            if (existing == null) return false;

            existing.totalUsages = usages;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCreatorCodeAsync(int id)
        {
            var existing = await _context.mstCreatorCodes.FindAsync(id);
            if (existing == null) return false;

            existing.isActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
