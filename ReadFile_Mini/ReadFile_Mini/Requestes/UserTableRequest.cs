

namespace ReadFile_Mini.Requestes
{
    public class UserTableRequest
    {
        
       // public int UserId { get; set; }
        public int TripId { get; set; }
        //public Trip? Trip { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal totalAmount { get; set; }
    }
}
