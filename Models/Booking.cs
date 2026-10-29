namespace FreightWiseApi.Models;

public class Booking
{
    public int Id { get; set; }
    public string BookingReference { get; set; }
    public string ShipperName { get; set; }
    public string ShipperEmail { get; set; }
    public string OriginPort { get; set; }
    public string DestinationPort { get; set; }
    public string ContainerType { get; set; }
    public decimal TotalAmountUSD { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

