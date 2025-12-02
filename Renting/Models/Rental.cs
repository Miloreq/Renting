namespace Renting.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int? AssetId { get; set; }
        public Asset? Asset { get; set; }
        public int? UserId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public Statusenum Status { get; set; }
        public string? Notes { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public DateOnly? UpdatedAt { get; set; }
    }
}
