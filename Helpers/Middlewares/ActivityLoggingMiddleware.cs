using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using CampaignManagement.Helpers.DbContexts;
using CampaignManagement.Models.Entity;

namespace CampaignManagement.Helpers.Middlewares
{
    public class ActivityLoggingMiddleware : IMiddleware
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly LinkGenerator _linkGenerator;

        public ActivityLoggingMiddleware(IServiceScopeFactory serviceScopeFactory, LinkGenerator linkGenerator)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _linkGenerator = linkGenerator;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Skip logging for root, auth pages and static files
            if (string.IsNullOrEmpty(path) || path == "/" || path.StartsWith("/api") || path.StartsWith("/auth/login") || path.StartsWith("/auth/logout") || path.StartsWith("/auth/otpverification") || path.StartsWith("/auth/verifyotp") || Path.HasExtension(path))
            {
                await next(context);
                return;
            }

            var authToken = context.Request.Cookies["cmAuthToken"];
            int? currentUserId = null;
            int accessLevel = 0;
            string issue = "";

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    var tokenInfo = TokenHelper.DecryptToken(authToken);
                    if (tokenInfo == null)
                    {
                        throw new InvalidOperationException("Invalid token payload");
                    }

                    currentUserId = tokenInfo.UserId;
                    accessLevel = tokenInfo.AccessLevel;

                    using var scope = _serviceScopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CampaignDbContext>();

                    var validUser = dbContext.mstUsers.FirstOrDefault(u => u.mstUserId == tokenInfo.UserId && u.userAccessLevel == tokenInfo.AccessLevel && u.isActive);

                    if (validUser == null)
                    {
                        issue = "Invalid user credentials";
                        context.Response.Cookies.Delete("cmAuthToken");

                        var logoutUrl = _linkGenerator.GetPathByAction("Logout", "Auth", values: null, pathBase: context.Request.PathBase);
                        context.Response.Redirect(logoutUrl ?? "/Auth/Logout");
                        return;
                    }
                }
                catch
                {
                    issue = "Token parsing failed";
                    context.Response.Cookies.Delete("cmAuthToken");

                    var logoutUrl = _linkGenerator.GetPathByAction("Logout", "Auth", values: null, pathBase: context.Request.PathBase);
                    context.Response.Redirect(logoutUrl ?? "/Auth/Logout");
                    return;
                }
            }
            else
            {
                var logoutUrl = _linkGenerator.GetPathByAction("Logout", "Auth", values: null, pathBase: context.Request.PathBase);
                context.Response.Redirect(logoutUrl ?? "/Auth/Logout");
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                issue = ex.Message;
                throw;
            }
            finally
            {
                stopwatch.Stop();

                if (accessLevel > 1)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CampaignDbContext>();
                    await SaveUserActivityAsync(context, currentUserId, issue, authToken, dbContext);
                }
            }
        }

        private async Task SaveUserActivityAsync(HttpContext context, int? userId, string reason, string token, CampaignDbContext dbContext)
        {
            var clientIP = context.Connection.RemoteIpAddress?.ToString();
            var method = context.Request.Method;
            var query = context.Request.QueryString.ToString();
            var path = context.Request.Path;

            var endpoint = context.GetEndpoint();
            var descriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            var controllerName = descriptor?.ControllerName ?? "Unknown";
            var actionName = descriptor?.ActionName ?? "Unknown";

            var activity = new trnUserActivity
            {
                userId = userId ?? 0,
                activityDateTime = DateTime.Now,
                requestMethod = method,
                queryParams = query ?? "",
                IPAddress = clientIP ?? "unknown",
                pageUrl = path,
                remarks = reason ?? "",
                functionName = $"{controllerName}/{actionName}"
            };

            dbContext.trnUserActivity.Add(activity);

            await dbContext.SaveChangesAsync();
        }
    }
}
