using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
