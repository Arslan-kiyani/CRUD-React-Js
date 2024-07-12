using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class InventoryDestination
    {
        [Key]
        public int id { get; set; }
        public int inventoryid { get; set; }
        public string parentid { get; set; }
        public string inventorycode { get; set; }
        public string startinv { get; set; }
        public string endinv { get; set; }
        public string invtypecode { get; set; }
        public int isroom { get; set; }
    }
}
