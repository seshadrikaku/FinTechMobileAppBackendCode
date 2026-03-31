namespace AuthService.Models
{
    public class RefreshTokenResponseDto
    {


        public Guid MobileUserId { get; set; }
        public string? AccessToken { get; set; }
        public string JwtToken { get; set; }






    }
}
