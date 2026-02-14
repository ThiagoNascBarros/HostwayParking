namespace HostwayParking.Communication.Response
{
    public class ResponseOccupancyByHourJson
    {
        public List<OccupancyByHourItem> Items { get; set; } = [];
    }

    public class OccupancyByHourItem
    {
        public int Hour { get; set; }
        public string HourRange { get; set; } = string.Empty;
        public double AverageVehicles { get; set; }
        public int MaxVehicles { get; set; }
    }
}
