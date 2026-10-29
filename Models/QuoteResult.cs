namespace FreightWiseApi.Models;

public class QuoteResult
{
    public string QuoteId { get; set; }
    public string OriginPort { get; set; }
    public string DestinationPort { get; set; }
    public string ContainerType { get; set; }
    public decimal BaseOceanFreight { get; set; }
    public decimal BunkerAdjustmentFactor { get; set; }
    public decimal TerminalHandlingCharge { get; set; }
    public decimal TotalRateUSD { get; set; }
    public int EstimatedTransitDays { get; set; }
    public string ValidUntilDate { get; set; }
}

