using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public int Members { get; set; }
    }
}
