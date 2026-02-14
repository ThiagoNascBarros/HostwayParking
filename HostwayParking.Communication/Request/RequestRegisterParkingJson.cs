namespace HostwayParking.Communication.Request
{
    public class RequestRegisterParkingJson
    {

        public string Code { get; set; } = string.Empty;
        public RequestAddressJson Address { get; set; } = new();
    }

    public class RequestAddressJson
    {
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Supplement { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;

    }
}