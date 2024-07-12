namespace ReadFile_Mini.Response
 {   public class TripResponse
    {
        public int TripId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public int Members { get; set; }
    }
}
