using AuthService.Data;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {

        private readonly AuthDbContext _dbContext;

        public AuthService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // 1️⃣ LOGIN → Send OTP
        public async Task<CommonApiResponse<LoginResponseDto>> OtpLoginAsync(LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null || string.IsNullOrEmpty(loginRequestDto.MobileNumber))
                throw new ArgumentException("Invalid login request");

            if (loginRequestDto.MobileNumber.Length != 10 || !loginRequestDto.MobileNumber.All(char.IsDigit))
                throw new ArgumentException("Invalid mobile number format");

            if (loginRequestDto.MobileNumber.StartsWith('0'))
                throw new ArgumentException("Mobile number should not start with 0");

            var otp = new Random().Next(100000, 999999).ToString();
            var userInDb = await _dbContext.MobileUsers.FirstOrDefaultAsync(u => u.MobileNumber == loginRequestDto.MobileNumber);

            Guid mobileUserId;

            if (userInDb == null)
            {
                // New user — create record
                mobileUserId = Guid.NewGuid();
                await _dbContext.MobileUsers.AddAsync(new MobileUsers
                {
                    MobileUserId = mobileUserId,
                    MobileNumber = loginRequestDto.MobileNumber,
                    CountryCode = loginRequestDto.CountryCode,
                    Otp = otp,
                    OtpAttempts = 1,
                    OtpGeneratedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    isActive = true,
                    isExistingUser = false
                });
            }

            else if(userInDb.isDeleted == true)
            {
                return new CommonApiResponse<LoginResponseDto>
                {
                    StatusCode = 400,
                    Message = "This mobile number is Deleted. Please contact support.",

                };
            }
            else if(userInDb.isActive == false)
            {
                return new CommonApiResponse<LoginResponseDto>
                {
                    StatusCode = 404,
                    Message = "User Not Found. Please contact support.",

                };
            }
            else
            {
                mobileUserId = userInDb.MobileUserId;

                // Check OTP attempts in the last 1 hour — max 3 allowed
                var oneHourAgo = DateTime.UtcNow.AddHours(-1);
                if (userInDb.OtpGeneratedAt >= oneHourAgo && userInDb.OtpAttempts >= 3)
                    return new CommonApiResponse<LoginResponseDto>
                    {
                        StatusCode = 400,
                        Message = "Maximum OTP attempts exceeded. Please try again after 1 hour.",

                    };


                // Reset counter if the 1-hour window has expired
                if (userInDb.OtpGeneratedAt < oneHourAgo)
                    userInDb.OtpAttempts = 0;

                userInDb.Otp = otp;
                userInDb.OtpAttempts += 1;
                userInDb.OtpGeneratedAt = DateTime.UtcNow;
                userInDb.UpdatedAt = DateTime.UtcNow;
                    userInDb.     isExistingUser = false;
            }

            await _dbContext.SaveChangesAsync();

            return new CommonApiResponse<LoginResponseDto>
            {
                StatusCode = 200,
                Message = "OTP sent successfully",
                Data = new LoginResponseDto
                {
                    MobileUserId = mobileUserId,
                    MobileNumber = loginRequestDto.MobileNumber,
                     CountryCode = loginRequestDto.CountryCode,
                    otp = otp
                   
                }
            };
        }






        // 2️⃣ VERIFY OTP → Login success + Token
        public async Task<CommonApiResponse<OtpVerificationResponseDto>> VerifyOtpAsync(OtpVerifyRequestDto otpVerifyRequestDto)
        {
          if(otpVerifyRequestDto.MobileUserId == Guid.Empty || string.IsNullOrEmpty(otpVerifyRequestDto.Otp))
            {
                return new CommonApiResponse<OtpVerificationResponseDto>
                {
                    StatusCode = 400,
                    Message = "Invalid OTP verification request",

                };
                
            }

            if(otpVerifyRequestDto.Otp.Length != 6 || !otpVerifyRequestDto.Otp.All(char.IsDigit))
            {
                return new CommonApiResponse<OtpVerificationResponseDto>
                {
                    StatusCode = 400,
                    Message = "Invalid OTP format",

                };
            }
            var UserInDb= await _dbContext.MobileUsers.FirstOrDefaultAsync(u => u.MobileUserId == otpVerifyRequestDto.MobileUserId );
            
            if(UserInDb == null || UserInDb.isDeleted == true)
            {
                return new CommonApiResponse<OtpVerificationResponseDto>
                {
                    StatusCode = 404,
                    Message = "User not found. Please contact support.",

                };
            }
            if(UserInDb.Otp != otpVerifyRequestDto.Otp)
            {
                return new CommonApiResponse<OtpVerificationResponseDto>
                {
                    StatusCode = 400,
                    Message = "Invalid OTP. Please try again.",
                };
            }


                UserInDb.IsVerified = true;
                UserInDb.DeviceToken = otpVerifyRequestDto.DeviceToken;
                UserInDb.FcmToken = otpVerifyRequestDto.FcmToken;
                UserInDb.Version = otpVerifyRequestDto.Version;
                UserInDb.UpdatedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                return new CommonApiResponse<OtpVerificationResponseDto>
                {
                    StatusCode = 200,
                    Message = "OTP verified successfully",
                    Data = new OtpVerificationResponseDto
                    {
                        MobileUserId = UserInDb.MobileUserId,
                        MobileNumber = UserInDb.MobileNumber,
                        CountryCode = UserInDb.CountryCode,
                        Name = UserInDb.Name,
                        Email = UserInDb.Email,
                        DeviceToken = UserInDb.DeviceToken,
                        FcmToken = UserInDb.FcmToken,
                        Version = UserInDb.Version,
                        isExistingUser = UserInDb.isExistingUser ?? false,
                        isVerified = UserInDb.IsVerified ?? false,
                        OtpGeneratedAt = UserInDb.OtpGeneratedAt ?? DateTime.MinValue,
                        OtpAttempts = UserInDb.OtpAttempts ?? 0,
                        otp = UserInDb.Otp
                    }
                };

            
            




        }

        // 3️⃣ GET USER DETAILS (after login)
        public Task<CommonApiResponse<UserDetailsResponseDto>> GetUserDetailsAsync()
        {
            throw new NotImplementedException();
        }

        // 4️⃣ REGISTER / UPDATE USER
        public Task<CommonApiResponse<RegisterResponseDto>> ManageUserDetailsAsync(RegisterRequestDto registerRequestDto)
        {
            throw new NotImplementedException();
        }

        // 5️⃣ REFRESH TOKEN
        public Task<CommonApiResponse<RefreshTokenResponseDto>> RefreshTokenAsync(string AccessToken)
        {
            throw new NotImplementedException();
        }

        // 6️⃣ UPDATE FCM TOKEN
        public Task<CommonApiResponse<bool>> UpdateFcmTokenAsync(string NewFcmToken)
        {
            throw new NotImplementedException();
        }

        // 7️⃣ LOGOUT
        public Task<CommonApiResponse<bool>> LogoutAsync(LogoutRequestDto logoutRequestDto)
        {
            throw new NotImplementedException();
        }

        // 8️⃣ APP VERSION (independent)
        public Task<CommonApiResponse<string>> GetAppVersionAsync()
        {
            throw new NotImplementedException();
        }


    }


}
