using System.ComponentModel.DataAnnotations;
using UniNest.API.Models.Enums;

namespace UniNest.API.Models.User
{
    public class RoommateProfile
    {
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        // Budget expressed as maximum monthly budget the student can pay
        public decimal? MonthlyBudget { get; set; }

        public Gender? PreferredGender { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        public DateTimeOffset? MoveInDate { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string? ContactNumber { get; set; }

        // Navigation to user
        public ApplicationUser? User { get; set; }
    }
}
