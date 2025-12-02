using System.Reflection.Metadata;

namespace Renting.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }

        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public int InventoryCode { get; set; }

        public Stanenum Condition { get; set; }

        public string? Location { get; set; }

        public Boolean IsActive { get; set; }

    }
}
