namespace AuthService.Dtos
{
    public class VerifyOtpRequestDto
    {
        public Guid MobileUserId { get; set; }
        public string Otp { get; set; } = null!;
        public string? FcmToken { get; set; }
        public string? DeviceToken { get; set; }
        public string? AppVersion { get; set; }
    }
}
