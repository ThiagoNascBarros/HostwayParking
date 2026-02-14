namespace HostwayParking.Communication.Response
{
    public class ResponseTopVehiclesByTimeJson
    {
        public List<TopVehicleByTimeItem> Items { get; set; } = [];
    }

    public class TopVehicleByTimeItem
    {
        public string Plate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int TotalSessions { get; set; }
        public string TotalTimeParked { get; set; } = string.Empty;
        public double TotalMinutes { get; set; }
    }
}
