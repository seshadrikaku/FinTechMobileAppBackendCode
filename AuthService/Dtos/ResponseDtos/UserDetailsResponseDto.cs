namespace AuthService.Dtos
{
    public class UserDetailsResponseDto
    {
        public Guid MobileUserId { get; set; }
        public string MobileNumber { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
    }
}
