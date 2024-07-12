using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class JournalByDate
    {
        [Key]
        public int id {  get; set; }
        public string InvoiceOne {  get; set; }
        public string InvoiceTwo { get; set; }
        public string HotelName { get; set; }
        public string Reservation { get; set; }
        public int Quantity { get; set; }
        public string InvoiceText { get; set; }
        public decimal Single { get; set; }
        public decimal Total { get; set; }
        public string User { get; set; }
        public int Room { get; set; }
        public string Category { get; set; }
        public string Guest { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
    }
}
