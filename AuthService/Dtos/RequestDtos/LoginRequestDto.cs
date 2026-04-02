namespace AuthService.Dtos
{
    public class SendOtpRequestDto
    {
        public string MobileNumber { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
    }
}
