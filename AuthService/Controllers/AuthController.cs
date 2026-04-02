using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Common;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        private IActionResult ApiResult<T>(ApiResponse<T> response) =>
            StatusCode(response.StatusCode, response);

        /// <summary>Sends a one-time password to the given mobile number.</summary>
        [HttpPost("send-otp")]
        [EnableRateLimiting("otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto request) =>
            ApiResult(await _authService.SendOtpAsync(request));

        /// <summary>Verifies the OTP and returns access + refresh tokens.</summary>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request) =>
            ApiResult(await _authService.VerifyOtpAsync(request));

        /// <summary>Completes user profile. Requires a valid access token.</summary>
        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request) =>
            ApiResult(await _authService.RegisterUserAsync(request));

        /// <summary>Returns the authenticated user's profile.</summary>
        [Authorize]
        [HttpGet("user-details")]
        public async Task<IActionResult> GetUserDetails() =>
            ApiResult(await _authService.GetUserDetailsAsync());

        /// <summary>Issues a new access + refresh token pair. Tokens go in the request body, never in the URL.</summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request) =>
            ApiResult(await _authService.RefreshTokenAsync(request));

        /// <summary>Invalidates the current session server-side. Requires a valid access token.</summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout() =>
            ApiResult(await _authService.LogoutAsync());

        /// <summary>Updates the FCM push notification token for the authenticated user.</summary>
        [Authorize]
        [HttpPatch("fcm-token")]
        public async Task<IActionResult> UpdateFcmToken([FromBody] UpdateFcmTokenRequestDto request) =>
            ApiResult(await _authService.UpdateFcmTokenAsync(request.FcmToken));

        /// <summary>Returns the minimum required app version. Public endpoint.</summary>
        [HttpGet("app-version")]
        public async Task<IActionResult> GetAppVersion() =>
            ApiResult(await _authService.GetAppVersionAsync());
    }
}
