namespace UniNest.API.Dtos.Auth;

public record AuthResponse
(
    string Token,
    UserResponse User
);