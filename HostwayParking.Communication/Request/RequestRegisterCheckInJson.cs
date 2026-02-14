namespace HostwayParking.Communication.Request
{
    public class RequestRegisterCheckInJson
    {
        public string Plate { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }
}
