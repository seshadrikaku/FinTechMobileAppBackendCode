namespace AuthService.Models
{
    public class MobileUsers
    {
        public int Id { get; set; }

        public Guid MobileUserId { get; set; }   // NOT NULL ✅

        public string MobileNumber { get; set; } // NOT NULL ✅

        public string? Gender { get; set; }

        public string? City { get; set; }

        public string? CountryCode { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Otp { get; set; }

        public int? OtpAttempts { get; set; }

        public DateTime? OtpGeneratedAt { get; set; }

        public bool? IsVerified { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeviceToken { get; set; }

        public string? AccessToken { get; set; }

        public string? FcmToken { get; set; }

        public string? Version { get; set; }

        public bool? isDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool? isActive { get; set; }

        public bool? isExistingUser { get; set; }
    }
}