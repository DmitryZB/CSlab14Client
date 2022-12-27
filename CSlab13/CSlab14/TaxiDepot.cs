#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSlab13
{
    public class TaxiDepot
    {
        [Key]
        public int Id { get; set; }
        public string? Address { get; set; }
        public List<TaxiGroup> TaxiGroups { get; set; } = new();
    }
}