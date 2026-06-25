namespace UniNest.API.Dtos.Auth;

public record GoogleUserInfo
(
    string Email,

    string Name,

    string? Picture,
    string? GoogleId
);