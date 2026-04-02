namespace AuthService.Dtos
{
    // Sent as POST body — never as query parameters (tokens must not appear in URLs/logs)
    public class RefreshTokenRequestDto
    {
        public Guid MobileUserId { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
