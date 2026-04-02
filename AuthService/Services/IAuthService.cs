using AuthService.Dtos;
using Shared.Common;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<SendOtpResponseDto>> SendOtpAsync(SendOtpRequestDto request);
        Task<ApiResponse<VerifyOtpResponseDto>> VerifyOtpAsync(VerifyOtpRequestDto request);
        Task<ApiResponse<RegisterUserResponseDto>> RegisterUserAsync(RegisterUserRequestDto request);
        Task<ApiResponse<UserDetailsResponseDto>> GetUserDetailsAsync();
        Task<ApiResponse<bool>> UpdateFcmTokenAsync(string fcmToken);
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<ApiResponse<string>> GetAppVersionAsync();
        Task<ApiResponse<bool>> LogoutAsync();
    }
}
