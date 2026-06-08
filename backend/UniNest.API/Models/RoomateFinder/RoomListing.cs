using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniNest.API.Models.Enums;
using UniNest.API.Models.User;

namespace UniNest.API.Models.RoomateFinder
{
    public class RoomListing
    {
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Column(TypeName = "numeric(10,2)")]
        public decimal MonthlyRent { get; set; }

        public int VacancyCount { get; set; }

        public Gender? PreferredGender { get; set; }

        public string? ContactNumber { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        // Soft-delete / active flag
        public bool IsActive { get; set; } = true;

        // Navigation
        public ApplicationUser? User { get; set; }

        public ICollection<ListingImage>? Images { get; set; }

        public ListingLocation? Location { get; set; }
    }
}
