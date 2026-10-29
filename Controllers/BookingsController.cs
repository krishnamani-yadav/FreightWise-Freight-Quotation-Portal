using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreightWiseApi.Data;
using FreightWiseApi.Models;

namespace FreightWiseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        var bookings = await _context.Bookings
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Booking request body is required." });
        }

        if (string.IsNullOrWhiteSpace(request.ShipperName))
        {
            return BadRequest(new { message = "Shipper company/contact name is required." });
        }

        if (string.IsNullOrWhiteSpace(request.ShipperEmail))
        {
            return BadRequest(new { message = "Shipper email address is required." });
        }

        // Generate unique booking reference in the format BKG-2026-XXXX
        string reference;
        do
        {
            reference = $"BKG-2026-{Random.Shared.Next(1000, 9999)}";
        } while (await _context.Bookings.AnyAsync(b => b.BookingReference == reference));

        var booking = new Booking
        {
            BookingReference = reference,
            ShipperName = request.ShipperName.Trim(),
            ShipperEmail = request.ShipperEmail.Trim(),
            OriginPort = request.OriginPort?.Trim() ?? "Standard Origin Port",
            DestinationPort = request.DestinationPort?.Trim() ?? "Standard Destination Port",
            ContainerType = string.IsNullOrWhiteSpace(request.ContainerType) ? "20ft Standard" : request.ContainerType.Trim(),
            TotalAmountUSD = request.TotalAmountUSD > 0 ? request.TotalAmountUSD : 1500.00m,
            Status = "Confirmed",
            CreatedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, booking);
    }
}

