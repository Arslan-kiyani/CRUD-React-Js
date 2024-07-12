using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class sdva
    {
        [Key]
        public int id { get; set; }
        public string EDI { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        
        public string Department { get; set; }
        public string SSN { get; set; }
    
        public string EMAIL_ADDRESS{ get; set; }
     
        public string PHONE_NUMBER { get; set; }
       
    }
}
