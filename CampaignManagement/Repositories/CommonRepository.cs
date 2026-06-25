using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Interfaces;

namespace CampaignManagement.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly CampaignDbContext _context;
        private readonly IConfiguration _configuration;

        public CommonRepository(CampaignDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public int iCheckForDuplicateEmailIdAndPhoneNumber(int iMstUserId, string emailId, string phoneNumber)
        {
            var existingUser = _context.mstUsers
                .FirstOrDefault(u => u.mstUserId != iMstUserId && (u.email == emailId || u.phoneNumber == phoneNumber));

            return existingUser != null ? 1 : 0;
        }

        public string GetBase64ImageAsync(string relativeImagePath)
        {
            try
            {
                var uploadFolder = _configuration["UploadSettings:CommonUploadFolder"] ?? "";
                var fullPath = Path.Combine(uploadFolder, relativeImagePath);

                if (File.Exists(fullPath))
                {
                    var bytes = File.ReadAllBytes(fullPath);
                    return Convert.ToBase64String(bytes);
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string ConvertPathToNetworkImage(string relativeImagePath)
        {
            var baseUrl = _configuration["ApplicationSettings:BaseUrl"] ?? "";
            return $"{baseUrl}{relativeImagePath}";
        }
    }
}
