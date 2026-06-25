using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.DTOs;

namespace CampaignManagement.Helpers.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public readonly IApiResponseRepository _apiResponseRepository;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, IApiResponseRepository apiResponseRepository)
        {
            _logger = logger;
            _apiResponseRepository = apiResponseRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                StreamWriter sw;
                string currentPath = Directory.GetCurrentDirectory();
                string folderRoute = Path.Combine(currentPath, "Logs");

                var fileName = "LogReport_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                string fileRoute = Path.Combine(folderRoute, fileName);

                if (!Directory.Exists(folderRoute))
                {
                    Directory.CreateDirectory(folderRoute);
                }

                if (!File.Exists(fileRoute))
                {
                    sw = File.CreateText(fileRoute);
                    sw.Flush();
                    sw.Close();
                }

                var endpoint = context.GetEndpoint();
                var descriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
                var controllerName = descriptor?.ControllerName;
                var actionName = descriptor?.ActionName;

                sw = File.AppendText(fileRoute);
                sw.WriteLine("*".PadRight(300, '*'));
                sw.WriteLine("");
                sw.WriteLine("Date       : {0}", DateTime.Now.ToString());
                sw.WriteLine("Controller : {0}", controllerName);
                sw.WriteLine("Action     : {0}", actionName);
                sw.WriteLine("Exception  : {0}", ex.ToString());
                sw.WriteLine("");
                sw.WriteLine("*".PadRight(300, '*'));
                sw.Flush();
                sw.Close();

                // Clean up log files older than 7 days
                Directory.GetFiles(folderRoute).Select(f => new FileInfo(f)).Where(f => f.CreationTime < DateTime.Now.AddDays(-7)).ToList().ForEach(f => f.Delete());

                var errorData = new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    ExceptionType = ex.GetType().Name
                };

                var problemDetails = _apiResponseRepository.FailureResponse(new ApiResponseDTO { data = errorData });

                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            }
        }
    }
}
