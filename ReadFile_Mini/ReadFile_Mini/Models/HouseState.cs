using System.ComponentModel.DataAnnotations;

namespace ReadFile_Mini.Models
{
    public class HouseState
    {
        [Key]
        public int id { get; set; }
        public string HotelName { get; set; }
        public string HouseDate { get; set; }
        public int Free { get; set; }
        public double FreePercentage { get; set; }  
        public double OccupiedPercentage { get; set; } 
        public double BedsPercentage { get; set; } 
        public int ArrivalsNo { get; set; } 
        public int ArrivalsPers {  get; set; }
        public int DeparturesNo {  get; set; } 
        public int DeparturesPers {  get; set; } 
        public int InHouseNo { get; set; }
        public int InHousePers {  get; set; }
        public decimal Acc {  get; set; }
        public  double ADR { get; set; }
        public double RevPar { get; set; }
        public int occupied{ get; set; }
        public decimal Accommodation { get; set; }
        public decimal FB { get; set; }
        public decimal Extras { get; set; }
        public decimal Total { get; set; }

    }
}
