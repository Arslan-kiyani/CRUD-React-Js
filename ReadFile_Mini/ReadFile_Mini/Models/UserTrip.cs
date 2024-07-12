using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class UserTrip
    {
        [Key]
        public int UserTrip_Id { get; set; }

        [ForeignKey("UserTable")]
        public int UserId { get; set; }
        public UserTable UserTable { get; set; }
        
        //public Trip Trip { get; set; }

       // public string? PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        //public decimal LeftAmount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
