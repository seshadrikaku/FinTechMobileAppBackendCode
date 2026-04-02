namespace AuthService.Dtos
{
    // OTP is NOT returned — it is delivered via SMS only
    public class SendOtpResponseDto
    {
        public Guid MobileUserId { get; set; }
        public string MobileNumber { get; set; } = null!;
        public string? CountryCode { get; set; }
        public string Otp { get; set; }
        
    }
}
