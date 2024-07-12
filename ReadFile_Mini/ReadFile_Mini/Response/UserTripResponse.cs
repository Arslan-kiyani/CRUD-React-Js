

namespace ReadFile_Mini.Response
{
    public class UserTripResponse
    {
        public int UserTrip_Id { get; set; }

        public int UserId { get; set; }
        //public UserTable UserTable { get; set; }

        //public int TripId { get; set; }
        //public string Name { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        //public int Members { get; set; }
        //public Trip Trip { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        //public string? PaymentStatus { get; set; }
        public decimal paidAmount { get; set; }
        public decimal totalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        
    }
}
