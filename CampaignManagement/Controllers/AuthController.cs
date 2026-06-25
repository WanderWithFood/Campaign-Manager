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
                catch { }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
            {
                ViewBag.Error = "Please enter email and password";
                return View();
            }

            var result = await _authRepository.LoginAsync(loginDTO.Email, loginDTO.Password);

            if (result.success)
            {
                var data = result.data as dynamic;
                var token = (string)data.token;

                Response.Cookies.Append("cmAuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = DateTimeOffset.Now.AddMinutes(30)
                });

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = result.message;
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
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(int userId, string otp)
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
    }
}
