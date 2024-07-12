using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class DestinationData
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
}
