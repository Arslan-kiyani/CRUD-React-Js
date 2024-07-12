using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class PersonalInfo
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Languages { get; set; }   
        public string Race { get; set; }

        public DateTime CreatedDate { get; set; }
    }

}
