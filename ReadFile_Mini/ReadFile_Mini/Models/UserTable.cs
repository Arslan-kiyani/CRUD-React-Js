using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class UserTable
    {
        [Key]
        public int UserId { get; set; }

        public int TripId { get; set; }
        //public Trip Trip { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public decimal PaidAmount { get; set; } 
        public decimal totalAmount { get; set; }
        //public decimal Balance { get; set; }

    }
}
