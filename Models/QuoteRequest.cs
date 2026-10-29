namespace FreightWiseApi.Models;

public class QuoteRequest
{
    public string OriginPortCode { get; set; }
    public string DestinationPortCode { get; set; }
    public string ContainerType { get; set; }
    public double CargoWeightTons { get; set; }
    public string CargoDescription { get; set; }
}

