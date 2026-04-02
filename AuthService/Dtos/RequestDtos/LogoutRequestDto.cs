namespace AuthService.Dtos
{
    // Logout requires [Authorize] — user ID comes from the JWT claim, no request body needed.
    public class UpdateFcmTokenRequestDto
    {
        public string FcmToken { get; set; } = null!;
    }
}
