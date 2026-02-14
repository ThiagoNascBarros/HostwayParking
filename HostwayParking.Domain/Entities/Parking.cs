using HostwayParking.Domain.Entities.Embedded;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostwayParking.Domain.Entities
{
    [Table("parking")]
    public class Parking
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(12)]
        public string Code { get; set; } = string.Empty;

        public List<Vehicle> Vehicles { get; set; } = [];

        public Address Address { get; set; }
    }
}
