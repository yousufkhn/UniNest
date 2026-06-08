using System.ComponentModel.DataAnnotations;

namespace UniNest.API.Models.RoomateFinder
{
    public class ListingLocation
    {
        public Guid Id { get; set; }

        public Guid RoomListingId { get; set; }

        [MaxLength(200)]
        public string? Area { get; set; }

        [MaxLength(200)]
        public string? Landmark { get; set; }

        [MaxLength(1000)]
        public string? FullAddress { get; set; }

        // Optional geolocation for distance-based filtering
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        // Navigation
        public RoomListing? RoomListing { get; set; }
    }
}
