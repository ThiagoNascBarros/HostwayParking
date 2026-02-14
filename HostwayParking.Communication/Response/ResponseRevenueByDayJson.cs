namespace HostwayParking.Communication.Response
{
    public class ResponseRevenueByDayJson
    {
        public List<RevenueByDayItem> Items { get; set; } = [];
        public decimal TotalRevenue { get; set; }
    }

    public class RevenueByDayItem
    {
        public DateOnly Date { get; set; }
        public int TotalSessions { get; set; }
        public decimal Revenue { get; set; }
    }
}
