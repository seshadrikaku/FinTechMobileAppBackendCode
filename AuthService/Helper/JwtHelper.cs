using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Infrastructure
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(string userId, string mobile)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.MobilePhone, mobile)
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["JwtSettings:AccessTokenExpireMinutes"] ?? "15")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string rawToken, string tokenHash) GenerateRefreshToken(int expiryDays)
        {
            var expiresAtUnixSeconds = DateTimeOffset.UtcNow.AddDays(expiryDays).ToUnixTimeSeconds();
            var rawBytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = $"{expiresAtUnixSeconds}.{Convert.ToBase64String(rawBytes)}";
            var tokenHash = HashValue(rawToken);
            return (rawToken, tokenHash);
        }

        public static bool TryGetRefreshTokenExpiry(string rawToken, out DateTime expiresAtUtc)
        {
            expiresAtUtc = default;

            if (string.IsNullOrWhiteSpace(rawToken))
                return false;

            var separatorIndex = rawToken.IndexOf('.');
            if (separatorIndex <= 0)
                return false;

            return long.TryParse(
                       rawToken[..separatorIndex],
                       NumberStyles.None,
                       CultureInfo.InvariantCulture,
                       out var expiresAtUnixSeconds)
                   && TryConvertFromUnixTime(expiresAtUnixSeconds, out expiresAtUtc);
        }

        private static bool TryConvertFromUnixTime(long unixSeconds, out DateTime expiresAtUtc)
        {
            try
            {
                expiresAtUtc = DateTimeOffset.FromUnixTimeSeconds(unixSeconds).UtcDateTime;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                expiresAtUtc = default;
                return false;
            }
        }

        public static string HashValue(string value) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    }
}
