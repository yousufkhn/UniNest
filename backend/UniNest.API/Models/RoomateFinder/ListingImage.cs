using System.ComponentModel.DataAnnotations;

namespace UniNest.API.Models.RoomateFinder
{
    public class ListingImage
    {
        public Guid Id { get; set; }

        public Guid RoomListingId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }

        // Optional cloud storage public id to support deletes/transformations
        public string? PublicId { get; set; }

        // Explicit ordering for image galleries
        public int Order { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        // Navigation
        public RoomListing? RoomListing { get; set; }
    }
}
