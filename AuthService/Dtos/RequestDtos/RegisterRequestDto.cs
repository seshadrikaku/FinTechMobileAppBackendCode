namespace AuthService.Dtos
{
    // MobileUserId is NOT in this DTO — it is extracted from the JWT claim in [Authorize] endpoint
    public class RegisterUserRequestDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string? Gender { get; set; }
        public string? City { get; set; }
    }
}
