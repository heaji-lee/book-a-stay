using BookAStay.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAStay.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options) {
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
}