using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renting.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int? AssetId { get; set; }
        public Asset? Assets { get; set; }

        [NotMapped]
        public string? AssetName;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Notes { get; set; }
        public Statusenum Status { get; set; }
        public DateTime? CheckedOutAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        // NOWE POLA
        public string? Condition { get; set; }
        public string? DamageNotes { get; set; }
    }
}
