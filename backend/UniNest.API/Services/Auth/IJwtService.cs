using UniNest.API.Models.User;

namespace UniNest.Api.Services.Auth;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}