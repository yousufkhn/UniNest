using UniNest.API.Dtos.Auth;

namespace UniNest.API.Services.Auth;

public interface IGoogleAuthService
{
    Task<AuthResponse> AuthenticateAsync(
        string idToken);
}