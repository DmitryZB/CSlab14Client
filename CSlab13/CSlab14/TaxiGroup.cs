using System.ComponentModel.DataAnnotations;

namespace CSlab13
{
    public class TaxiGroup
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int TaxiDepotId { get; set; }
        public TaxiDepot TaxiDepot { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        
    }
}