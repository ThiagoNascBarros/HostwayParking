using Microsoft.EntityFrameworkCore;

namespace HostwayParking.Domain.Entities.Embedded
{
    [Owned]
    public class Address
    {

        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Supplement { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;

    }
}
