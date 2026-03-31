namespace AuthService.Models
{
    public class OtpVerifyRequestDto
    {

        public Guid MobileUserId { get; set; }

        public string FcmToken { get; set; }

        public string DeviceToken { get; set; }
        public string Otp { get; set; }

        public string Version { get; set; }
       
    }
}
