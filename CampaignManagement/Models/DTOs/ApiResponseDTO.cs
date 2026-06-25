namespace CampaignManagement.Models.DTOs
{
    public class ApiResponseDTO
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public int statusCode { get; set; } = 200;

        public static ApiResponseDTO SuccessResponse(string message, int statusCode = 200)
        {
            return new ApiResponseDTO
            {
                success = true,
                message = message,
                statusCode = statusCode
            };
        }

        public static ApiResponseDTO FailureResponse(string message, int statusCode = 400)
        {
            return new ApiResponseDTO
            {
                success = false,
                message = message,
                statusCode = statusCode
            };
        }

        public static ApiResponseDTO SuccessResponse(object data, string message, int statusCode = 200)
        {
            return new ApiResponseDTO
            {
                success = true,
                message = message,
                data = data,
                statusCode = statusCode
            };
        }
    }
}
