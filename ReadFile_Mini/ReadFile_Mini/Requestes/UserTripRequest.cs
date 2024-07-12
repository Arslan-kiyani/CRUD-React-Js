using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Requestes
{
    public class UserTripRequest
    {
        //public int UserTrip_Id { get; set; }

        public int UserId { get; set; }
        //public UserTable UserTable { get; set; }

       // public string?  PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        //public decimal LeftAmount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
