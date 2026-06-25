using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniNest.Api.Services.Auth;
using UniNest.API.Dtos.Auth;
using UniNest.API.Models.User;

namespace UniNest.API.Services.Auth;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public GoogleAuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<AuthResponse> AuthenticateAsync(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            throw new ArgumentException(
                "Google id token is required.",
                nameof(idToken));
        }

        var clientId = _configuration["Google:ClientId"]
            ?? _configuration["Authentication:Google:ClientId"];

        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidOperationException(
                "Google ClientId is not configured.");
        }

        GoogleJsonWebSignature.Payload payload;

        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            };

            payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                settings);
        }
        catch (InvalidJwtException)
        {
            throw new UnauthorizedAccessException(
                "Invalid Google ID token.");
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException(
                "Google authentication failed.");
        }

        if (!payload.EmailVerified)
        {
            throw new UnauthorizedAccessException(
                "Google email is not verified.");
        }

        if (string.IsNullOrWhiteSpace(payload.Email))
        {
            throw new InvalidOperationException(
                "Google token did not contain an email address.");
        }

        var googleUser = new GoogleUserInfo
        (
            payload.Email,
            string.IsNullOrWhiteSpace(payload.Name)
                ? payload.Email
                : payload.Name,
            payload.Picture,
            payload.Subject
        );

        // Prefer GoogleId lookup
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.GoogleId == googleUser.GoogleId);

        // Fallback to email lookup
        if (user is null)
        {
            user = await _userManager
                .FindByEmailAsync(googleUser.Email);
        }

        // Create user if first login
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = googleUser.Email,
                Email = googleUser.Email,
                EmailConfirmed = true,

                FullName = googleUser.Name,
                ProfilePictureUrl = googleUser.Picture,

                CreatedAt = DateTimeOffset.UtcNow,

                IsCollegeVerified = false,

                GoogleId = googleUser.GoogleId
            };

            var createResult =
                await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    createResult.Errors
                        .Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Failed to create Google user: {errors}");
            }
        }
        else
        {
            // Backfill GoogleId for old accounts
            if (string.IsNullOrWhiteSpace(user.GoogleId))
            {
                user.GoogleId = googleUser.GoogleId;

                await _userManager.UpdateAsync(user);
            }
        }

        var token = _jwtService.GenerateToken(user);

        return new AuthResponse
        (
            token,

            new UserResponse
            (
                user.Id,
                user.Email!,
                user.FullName,
                user.ProfilePictureUrl
            )
        );
    }
}