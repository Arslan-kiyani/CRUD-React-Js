using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class SourceData
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
