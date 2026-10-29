using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreightWiseApi.Data;
using FreightWiseApi.Models;

namespace FreightWiseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuotesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("ports")]
    public async Task<ActionResult<IEnumerable<Port>>> GetPorts()
    {
        var ports = await _context.Ports
            .OrderBy(p => p.PortName)
            .ToListAsync();

        return Ok(ports);
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<QuoteResult>> CalculateQuote([FromBody] QuoteRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Quote request payload is required." });
        }

        if (string.IsNullOrWhiteSpace(request.OriginPortCode) || string.IsNullOrWhiteSpace(request.DestinationPortCode))
        {
            return BadRequest(new { message = "Both origin and destination port codes must be provided." });
        }

        if (string.Equals(request.OriginPortCode.Trim(), request.DestinationPortCode.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new { message = "Origin and destination ports cannot be the same." });
        }

        var origin = await _context.Ports.FirstOrDefaultAsync(p => p.PortCode == request.OriginPortCode.Trim());
        var destination = await _context.Ports.FirstOrDefaultAsync(p => p.PortCode == request.DestinationPortCode.Trim());

        if (origin == null || destination == null)
        {
            return BadRequest(new { message = "One or more specified ports could not be found." });
        }

        // Determine base ocean freight and transit days deterministically based on port pair
        var (baseRate, transitDays) = GetPortPairBaseMetrics(origin.PortCode, destination.PortCode);

        // Multiplier based on container type
        decimal containerMultiplier = request.ContainerType?.Trim().ToLowerInvariant() switch
        {
            "20ft standard" => 1.00m,
            "40ft standard" => 1.55m,
            "40ft high cube" => 1.70m,
            "20ft reefer" => 2.10m,
            "40ft reefer" => 2.45m,
            _ => 1.25m
        };

        // Heavy weight cargo adjustment (above 15 tons)
        decimal weightSurcharge = 0m;
        if (request.CargoWeightTons > 15)
        {
            weightSurcharge = (decimal)(request.CargoWeightTons - 15) * 15.00m;
        }

        decimal calculatedBaseOceanFreight = Math.Round((baseRate * containerMultiplier) + weightSurcharge, 2);
        decimal bunkerAdjustmentFactor = 175.00m; // Deterministic standard BAF as required
        decimal terminalHandlingCharge = 125.00m; // Deterministic standard THC as required
        decimal totalRateUSD = calculatedBaseOceanFreight + bunkerAdjustmentFactor + terminalHandlingCharge;

        var result = new QuoteResult
        {
            QuoteId = $"QUO-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
            OriginPort = $"{origin.PortName} ({origin.PortCode})",
            DestinationPort = $"{destination.PortName} ({destination.PortCode})",
            ContainerType = string.IsNullOrWhiteSpace(request.ContainerType) ? "20ft Standard" : request.ContainerType,
            BaseOceanFreight = calculatedBaseOceanFreight,
            BunkerAdjustmentFactor = bunkerAdjustmentFactor,
            TerminalHandlingCharge = terminalHandlingCharge,
            TotalRateUSD = totalRateUSD,
            EstimatedTransitDays = transitDays,
            ValidUntilDate = DateTime.UtcNow.AddDays(14).ToString("yyyy-MM-dd")
        };

        return Ok(result);
    }

    private static (decimal BaseRate, int TransitDays) GetPortPairBaseMetrics(string portA, string portB)
    {
        // Sort keys to provide deterministic symmetric baseline
        var key = string.Compare(portA, portB, StringComparison.OrdinalIgnoreCase) < 0
            ? $"{portA}-{portB}"
            : $"{portB}-{portA}";

        return key.ToUpperInvariant() switch
        {
            "CNSHA-USLAX" => (1850m, 15),
            "CNSHA-NLRTM" => (2150m, 28),
            "CNSHA-SGSIN" => (650m, 7),
            "CNSHA-INNSA" => (1250m, 16),
            "AEJEA-CNSHA" => (1450m, 18),

            "NLRTM-USLAX" => (2300m, 24),
            "SGSIN-USLAX" => (1750m, 22),
            "INNSA-USLAX" => (2100m, 30),
            "AEJEA-USLAX" => (2250m, 32),

            "NLRTM-SGSIN" => (1600m, 22),
            "INNSA-NLRTM" => (1350m, 19),
            "AEJEA-NLRTM" => (1200m, 15),

            "INNSA-SGSIN" => (750m, 8),
            "AEJEA-SGSIN" => (950m, 11),

            "AEJEA-INNSA" => (580m, 5),

            // Fallback for any other arbitrary combinations
            _ => (
                1100m + (Math.Abs(portA.GetHashCode() ^ portB.GetHashCode()) % 800),
                10 + (Math.Abs(portA.GetHashCode() + portB.GetHashCode()) % 20)
            )
        };
    }
}

