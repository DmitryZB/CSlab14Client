#nullable enable
using System.ComponentModel.DataAnnotations;

namespace CSlab13
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SitCounter { get; set; }
    }
}