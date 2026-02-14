using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostwayParking.Domain.Entities
{
    [Table("vehicle")]
    public class Vehicle
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(7)]
        public string Plate { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

    }
}