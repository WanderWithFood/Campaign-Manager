using CampaignManagement.Helpers;
using CampaignManagement.Helpers.Middlewares;
using CampaignManagement.Interfaces;
using CampaignManagement.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CampaignManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to dashboard/home
            var authToken = Request.Cookies["cmAuthToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    var tokenInfo = TokenHelper.DecryptToken(authToken);
                    if (tokenInfo != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    // Log exception and continue to login page
                    Console.WriteLine($"Token decryption error: {ex.Message}");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
            {
                ViewBag.Error = "Please enter email and password";
                return View();
            }

            var result = await _authRepository.LoginAsync(loginDTO.Email, loginDTO.Password);

            if (result.success && result.data != null)
            {
                try
                {
                    var data = result.data as dynamic;
                    var token = (string)data.token;

                    if (string.IsNullOrEmpty(token))
                    {
                        ViewBag.Error = "Authentication token is invalid";
                        return View();
                    }

                    Response.Cookies.Append("cmAuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        Expires = DateTimeOffset.Now.AddMinutes(30),
                        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                        Secure = true
                    });

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error processing authentication token";
                    Console.WriteLine($"Login error: {ex.Message}");
                    return View();
                }
            }

            ViewBag.Error = result.message ?? "Login failed. Please try again.";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("cmAuthToken");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult OtpVerification(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }

            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(int userId, string otp)
        {
            if (userId <= 0 || string.IsNullOrEmpty(otp))
            {
                ViewBag.Error = "Invalid user ID or OTP";
                ViewBag.UserId = userId;
                return View("OtpVerification");
            }

            try
            {
                var result = await _authRepository.VerifyOTPAsync(userId, otp);

                if (result)
                {
                    return RedirectToAction("Login");
                }

                ViewBag.Error = "Invalid or expired OTP";
                ViewBag.UserId = userId;
                return View("OtpVerification");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error verifying OTP";
                ViewBag.UserId = userId;
                Console.WriteLine($"OTP verification error: {ex.Message}");
                return View("OtpVerification");
            }
        }
    }
}
