namespace AuthService.Models
{
    public class MobileUser
    {
        public int Id { get; set; }
        public Guid MobileUserId { get; set; }
        public string MobileNumber { get; set; } = null!;
        public string? CountryCode { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
        public string? DateOfBirth { get; set; }

        // Legacy OTP value stored in the current [Otp] column.
        public string? Otp { get; set; }
        public int OtpAttempts { get; set; }
        public DateTime? OtpGeneratedAt { get; set; }

        // Auth state
        public bool IsVerified { get; set; }
        public bool IsExistingUser { get; set; }

        // Stored as a SHA-256 hash in the legacy [RefreshToken] column.
        public string? RefreshTokenHash { get; set; }

        // Device info
        public string? DeviceToken { get; set; }
        public string? FcmToken { get; set; }
        // Stored in the legacy [Version] column.
        public string? LastKnownAppVersion { get; set; }

        // Soft delete & status
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
