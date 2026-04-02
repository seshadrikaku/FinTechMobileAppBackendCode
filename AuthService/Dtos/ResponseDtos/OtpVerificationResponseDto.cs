namespace AuthService.Dtos
{
    public class VerifyOtpResponseDto
    {
        public Guid MobileUserId { get; set; }
        public string MobileNumber { get; set; } = null!;
        public string? CountryCode { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsExistingUser { get; set; }
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
