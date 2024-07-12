

namespace ReadFile_Mini.Response
{
    public class UserTableResponse
    {
        public int UserId { get; set; }


         //public int TripId { get; set; }
        //public string? TName { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public decimal Amount { get; set; }
        //public int Members { get; set; }

       // public List<Trip> Trip { get; set; }
        public decimal Balance { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal totalAmount { get; set; }
    }
}
