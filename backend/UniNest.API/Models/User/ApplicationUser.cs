using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using UniNest.API.Models.RoomateFinder;

namespace UniNest.API.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsCollegeVerified { get; set; }
        // Keep ApplicationUser minimal — feature data lives in separate tables.
    }
}
