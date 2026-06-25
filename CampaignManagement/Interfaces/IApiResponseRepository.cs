using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Interfaces
{
    public interface IApiResponseRepository
    {
        ApiResponseDTO SuccessResponse(ApiResponseDTO responseInfo);
        ApiResponseDTO FailureResponse(ApiResponseDTO responseInfo);
        ApiResponseDTO UnauthorizedResponse(ApiResponseDTO responseInfo);
    }
}
