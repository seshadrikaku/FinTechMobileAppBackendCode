# AuthService Changes Summary

## Overview

This document summarizes the auth-related changes currently applied in the project.

## Main Changes

### 1. Auth API Restructure

The auth controller now uses cleaner REST-style routes under:

- `POST /api/auth/send-otp`
- `POST /api/auth/verify-otp`
- `POST /api/auth/register`
- `GET /api/auth/me`
- `POST /api/auth/refresh-token`
- `POST /api/auth/logout`
- `PATCH /api/auth/fcm-token`
- `GET /api/auth/app-version`

File:

- `AuthService/Controllers/AuthController.cs`

### 2. Shared API Response Format Updated

The common API response contract was changed from:

- `success`

to:

- `statusCode`

Current response shape:

```json
{
  "statusCode": 200,
  "message": "OTP sent successfully.",
  "data": {},
  "errors": []
}
```

Also, controller actions now return the actual HTTP status code from the response object instead of always returning `200 OK`.

Files:

- `Shared.Common/CommonApiResponse.cs`
- `AuthService/Controllers/AuthController.cs`
- `AuthService/Middleware/ExceptionHandlingMiddleware.cs`
- `AuthService/Program.cs`

### 3. Existing Database Table Reused

The auth service was aligned to work with the existing table:

- `[MobileApp].[dbo].[MobileUsers]`

No new columns are required for the current implementation.

The C# model now maps to the existing legacy columns:

- `Otp` -> OTP value
- `OtpAttempts` -> failed OTP verification attempts
- `RefreshToken` -> hashed refresh token
- `Version` -> app version
- `isActive` -> active flag
- `isDeleted` -> soft delete flag
- `isExistingUser` -> profile completion flag

Files:

- `AuthService/Models/MobileUser.cs`
- `AuthService/Data/AuthDbContext.cs`

### 4. Model Refactor

The auth entity was moved to a singular model:

- `MobileUser`

instead of the older:

- `MobileUsers`

Current model supports:

- mobile-based OTP login
- profile registration
- refresh token storage
- FCM token updates
- soft delete and active/inactive status

Files:

- `AuthService/Models/MobileUser.cs`
- `AuthService/Models/MobileUsers.cs`

### 5. OTP Flow Updated

The send OTP and verify OTP logic was rewritten.

Current behavior:

- validates mobile number format
- creates new user record if mobile number does not exist
- regenerates OTP for existing active user
- OTP expires in 5 minutes
- maximum failed verification attempts: 5
- clears OTP after successful verification

File:

- `AuthService/Services/AuthService.cs`

### 6. JWT and Refresh Token Improvements

JWT handling was improved with a dedicated helper/service.

Current behavior:

- generates JWT access token
- generates refresh token with embedded expiry metadata
- stores hashed refresh token in DB
- validates refresh token using hash comparison
- rotates refresh token on refresh
- clears refresh token on logout

File:

- `AuthService/Helper/JwtHelper.cs`

### JWT Explanation

JWT is used here for authenticated APIs after OTP verification.

How it works in this project:

- after OTP is verified, the backend generates an access token
- the access token contains claims such as:
  - user id in `ClaimTypes.NameIdentifier`
  - mobile number in `ClaimTypes.MobilePhone`
- the token is signed using the secret key from `JwtSettings:Key`
- issuer and audience are validated using:
  - `JwtSettings:Issuer`
  - `JwtSettings:Audience`
- access token expiry is controlled by `JwtSettings:AccessTokenExpireMinutes`

Refresh token flow:

- a refresh token is generated separately from the access token
- the raw refresh token is returned to the client
- only the SHA-256 hash of the refresh token is stored in the DB
- refresh token includes expiry metadata inside the raw token itself
- during refresh:
  - incoming refresh token is hashed
  - hash is compared with DB value
  - embedded expiry is checked
  - if valid, a new access token and a new refresh token are issued

Why this is useful:

- access token is short-lived
- refresh token allows session continuation without OTP login every time
- DB stores hashed refresh token instead of raw value
- JWT claims are used by `[Authorize]` endpoints to identify the user

Related files:

- `AuthService/Helper/JwtHelper.cs`
- `AuthService/Program.cs`
- `AuthService/Services/AuthService.cs`

### 7. Request/Response DTO Cleanup

The auth request and response DTOs were cleaned up and aligned with the current API flow.

This includes DTOs for:

- send OTP
- verify OTP
- refresh token
- register user
- logout
- user profile response

Files:

- `AuthService/Dtos/RequestDtos/*`
- `AuthService/Dtos/ResponseDtos/*`

### 8. Startup and Middleware Improvements

The application startup was updated to include:

- JWT authentication
- authorization
- rate limiting
- CORS policy
- Swagger JWT support
- global exception handling middleware
- security headers

Note:

- The previous `AuthDatabaseSchemaInitializer` startup dependency is no longer used.
- Manual SQL cleanup is expected instead of automatic DB-fix logic at startup.

Files:

- `AuthService/Program.cs`
- `AuthService/Middleware/ExceptionHandlingMiddleware.cs`

### Exception Handling Explanation

Global exception handling is implemented through custom middleware.

How it works:

- `ExceptionHandlingMiddleware` runs early in the pipeline
- it wraps the next middleware/controller call in a `try/catch`
- if any unhandled exception occurs:
  - the error is logged with request method and request path
  - the API returns a JSON response in the common response format
  - HTTP status code is set to `500`

Returned structure:

```json
{
  "statusCode": 500,
  "message": "An unexpected error occurred. Please try again later.",
  "data": null,
  "errors": []
}
```

Development behavior:

- in Development environment, exception type and message are also added to the `errors` array
- this helps debugging without changing controller code

Why this is useful:

- prevents raw .NET exception pages from leaking to API clients
- keeps error responses consistent across endpoints
- centralizes logging and unexpected failure handling in one place

Related files:

- `AuthService/Middleware/ExceptionHandlingMiddleware.cs`
- `AuthService/Program.cs`

## Important Notes

### OTP Storage

Because the current `Otp` column is part of the existing schema, OTP is currently stored in that column directly.

If stronger security is needed later, a new dedicated hash-friendly OTP column should be added and the code can be updated to store hashed OTP values instead.

### Refresh Token Storage

Refresh token is stored hashed inside the existing `RefreshToken` column.

### Status Code Response Contract

Response payload now uses `statusCode` in the body, and controller responses are aligned with actual HTTP status codes.

## Manual DB Expectation

The current code assumes the existing `MobileUsers` table remains in use.

Recommended one-time manual cleanup in the DB:

- set null `OtpAttempts` to `0`
- set null `isActive` to `1`
- set null `isDeleted` to `0`
- set null `isExistingUser` to `0`
- set null `IsVerified` to `0`
- hash old values in `RefreshToken` if plain values already exist

## Build Status

The project was rebuilt successfully after these changes.

## Primary Files Changed

- `AuthService/Controllers/AuthController.cs`
- `AuthService/Data/AuthDbContext.cs`
- `AuthService/Helper/JwtHelper.cs`
- `AuthService/Middleware/ExceptionHandlingMiddleware.cs`
- `AuthService/Models/MobileUser.cs`
- `AuthService/Program.cs`
- `AuthService/Services/AuthService.cs`
- `AuthService/Services/IAuthService.cs`
- `Shared.Common/CommonApiResponse.cs`
