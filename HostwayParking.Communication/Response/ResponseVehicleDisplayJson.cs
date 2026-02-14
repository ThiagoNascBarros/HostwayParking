namespace HostwayParking.Communication.Response
{
    public class ResponseVehicleDisplayJson
    {
        public string Plate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public DateTime EntryTime { get; set; }
    }
}
