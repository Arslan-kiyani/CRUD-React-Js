using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public string ParentId { get; set; }
        public string InventoryCode  { get; set; }
        public string StartInv { get; set; }
        public string EndInv { get; set; }
        public string InvTypeCode { get; set; }
        public int IsRoom { get; set; }


    }
}
