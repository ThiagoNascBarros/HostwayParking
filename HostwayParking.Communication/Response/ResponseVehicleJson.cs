namespace HostwayParking.Communication.Response
{
    public class ResponseVehicleJson
    {
        public int Id { get; set; }
        public string Plate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
