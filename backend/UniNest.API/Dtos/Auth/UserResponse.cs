namespace UniNest.API.Dtos.Auth;

public record UserResponse
(
    string Id,

    string Email,

    string FullName,

    string? ProfilePictureUrl
);