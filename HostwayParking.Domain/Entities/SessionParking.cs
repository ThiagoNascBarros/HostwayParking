namespace HostwayParking.Domain.Entities
{
    public class SessionParking
    {

        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public decimal? AmountCharged { get; set; }
        public bool IsActive => ExitTime == null;

        public SessionParking() { }

        public SessionParking(int vehicleId)
        {
            VehicleId = vehicleId;
            EntryTime = DateTime.Now;
        }

        public void CloseSession(decimal price)
        {
            ExitTime = DateTime.Now;
            AmountCharged = price;
        }

    }
}
