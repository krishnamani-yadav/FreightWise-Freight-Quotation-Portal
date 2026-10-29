using Microsoft.EntityFrameworkCore;
using FreightWiseApi.Models;

namespace FreightWiseApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Port> Ports { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public static void SeedData(AppDbContext context)
    {
        if (!context.Ports.Any())
        {
            var ports = new List<Port>
            {
                new Port { Id = 1, PortCode = "INNSA", PortName = "Nhava Sheva (JNPT)", Country = "India" },
                new Port { Id = 2, PortCode = "SGSIN", PortName = "Port of Singapore", Country = "Singapore" },
                new Port { Id = 3, PortCode = "AEJEA", PortName = "Jebel Ali Port (Dubai)", Country = "United Arab Emirates" },
                new Port { Id = 4, PortCode = "NLRTM", PortName = "Port of Rotterdam", Country = "Netherlands" },
                new Port { Id = 5, PortCode = "USLAX", PortName = "Port of Los Angeles", Country = "United States" },
                new Port { Id = 6, PortCode = "CNSHA", PortName = "Port of Shanghai", Country = "China" }
            };

            context.Ports.AddRange(ports);
            context.SaveChanges();
        }

        if (!context.Bookings.Any())
        {
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    BookingReference = "BKG-2026-1042",
                    ShipperName = "Apex Global Logistics Ltd.",
                    ShipperEmail = "freight@apexglobal.com",
                    OriginPort = "Port of Shanghai (CNSHA)",
                    DestinationPort = "Port of Los Angeles (USLAX)",
                    ContainerType = "40ft High Cube",
                    TotalAmountUSD = 2450.00m,
                    Status = "Confirmed",
                    CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(-4)
                },
                new Booking
                {
                    Id = 2,
                    BookingReference = "BKG-2026-1088",
                    ShipperName = "Nordic Maritime Traders",
                    ShipperEmail = "supply-chain@nordicmaritime.de",
                    OriginPort = "Port of Singapore (SGSIN)",
                    DestinationPort = "Port of Rotterdam (NLRTM)",
                    ContainerType = "20ft Standard",
                    TotalAmountUSD = 1850.00m,
                    Status = "In Transit",
                    CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-7)
                }
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}

