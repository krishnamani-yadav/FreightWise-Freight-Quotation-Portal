namespace FreightWiseApi.Models;

public class BookingRequest
{
    public string ShipperName { get; set; }
    public string ShipperEmail { get; set; }
    public string OriginPort { get; set; }
    public string DestinationPort { get; set; }
    public string ContainerType { get; set; }
    public decimal TotalAmountUSD { get; set; }
}

