namespace CampaignManagement.Interfaces
{
    public interface ICommonRepository
    {
        int iCheckForDuplicateEmailIdAndPhoneNumber(int iMstUserId, string emailId, string phoneNumber);
        string GetBase64ImageAsync(string relativeImagePath);
        string ConvertPathToNetworkImage(string relativeImagePath);
    }
}
