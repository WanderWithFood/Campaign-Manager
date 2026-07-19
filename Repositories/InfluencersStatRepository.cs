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
    public class InfluencersStatRepository : IInfluencersStatRepository
    {
        private readonly CampaignDbContext _context;

        public InfluencersStatRepository(CampaignDbContext context)
        {
            _context = context;
        }

        public async Task<List<mstInfluencer>> GetInfluencersAsync(string? search = null)
        {
            var query = _context.mstInfluencers.Where(i => i.isActive);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.name.Contains(search) || 
                                         (i.category != null && i.category.Contains(search)) || 
                                         (i.location != null && i.location.Contains(search)));
            }
            return await query.ToListAsync();
        }

        public async Task<mstInfluencer?> GetInfluencerByIdAsync(int id)
        {
            return await _context.mstInfluencers
                .FirstOrDefaultAsync(i => i.mstInfluencerId == id && i.isActive);
        }

        public async Task<mstInfluencer> SaveInfluencerAsync(mstInfluencer influencer)
        {
            if (influencer.mstInfluencerId == 0)
            {
                influencer.created_at = DateTime.Now;
                influencer.isActive = true;
                
                // Auto-generate Creator ID: CH{platform prefix}{next sequential number}
                string platformPrefix = "YT"; // default to YouTube
                if (!string.IsNullOrEmpty(influencer.socialMediaPlatforms))
                {
                    var platforms = influencer.socialMediaPlatforms.ToLower();
                    if (platforms.Contains("instagram")) platformPrefix = "IG";
                    else if (platforms.Contains("youtube")) platformPrefix = "YT";
                    else if (platforms.Contains("twitter") || platforms.Contains("x")) platformPrefix = "TW";
                    else if (platforms.Contains("facebook")) platformPrefix = "FB";
                }
                
                int nextId = await _context.mstInfluencers.CountAsync() + 1;
                influencer.creatorId = $"CH{platformPrefix}{nextId:D2}";
                
                if (influencer.dateOfOnboarding == null)
                {
                    influencer.dateOfOnboarding = DateTime.Now;
                }
                
                _context.mstInfluencers.Add(influencer);
            }
            else
            {
                var existing = await _context.mstInfluencers.FindAsync(influencer.mstInfluencerId);
                if (existing != null)
                {
                    existing.name = influencer.name;
                    existing.category = influencer.category;
                    existing.niche = influencer.niche;
                    existing.location = influencer.location;
                    existing.dateOfBirth = influencer.dateOfBirth;
                    existing.gender = influencer.gender;
                    existing.socialMediaPlatforms = influencer.socialMediaPlatforms;
                    existing.managerName = influencer.managerName;
                    existing.managerNumber = influencer.managerNumber;
                    existing.instagramProfile = influencer.instagramProfile;
                    existing.whatsAppContact = influencer.whatsAppContact;
                    existing.integrationRequirements = influencer.integrationRequirements;
                    existing.exclusivityClause = influencer.exclusivityClause;
                    existing.contentDurationClause = influencer.contentDurationClause;
                    existing.paymentTerms = influencer.paymentTerms;
                    existing.followers = influencer.followers;
                    existing.engagement = influencer.engagement;
                    existing.avgViews = influencer.avgViews;
                    existing.estConversion = influencer.estConversion;
                    existing.estCostMin = influencer.estCostMin;
                    existing.estCostMax = influencer.estCostMax;
                    existing.reliabilityScore = influencer.reliabilityScore;
                    existing.notes = influencer.notes;
                    
                    // Map new profile fields
                    existing.phoneNumber = influencer.phoneNumber;
                    existing.shortDescription = influencer.shortDescription;
                    existing.languagesFamiliar = influencer.languagesFamiliar;
                    existing.profilePicturePath = influencer.profilePicturePath;
                    existing.instagramUrl = influencer.instagramUrl;
                    existing.residentialAddress = influencer.residentialAddress;
                    existing.dateOfOnboarding = influencer.dateOfOnboarding;
                    existing.influencerInterests = influencer.influencerInterests;
                    existing.paymentDetails = influencer.paymentDetails;
                }
            }
            await _context.SaveChangesAsync();
            return influencer;
        }

        public async Task<bool> AddNoteAsync(int influencerId, string note)
        {
            var existing = await _context.mstInfluencers.FindAsync(influencerId);
            if (existing == null) return false;
            
            existing.notes = string.IsNullOrEmpty(existing.notes) 
                ? note 
                : $"{existing.notes}\n{note}";
                
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCreatorCodeUsagesAsync(int influencerId)
        {
            return await _context.mstCreatorCodes
                .Where(c => c.influencerId == influencerId && c.isActive)
                .SumAsync(c => c.totalUsages);
        }
    }
}
