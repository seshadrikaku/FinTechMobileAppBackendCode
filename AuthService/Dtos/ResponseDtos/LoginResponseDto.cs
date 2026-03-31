namespace AuthService.Models
{
    public class LoginResponseDto
    {


        public Guid MobileUserId { get; set; }
        public string MobileNumber { get; set; }

        public string CountryCode { get; set; }

        public string otp { get; set; }




    }
}
