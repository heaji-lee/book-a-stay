using BookAStay.Data;
using BookAStay.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAStay.Services;

public class TestDataService(AppDbContext context) {
    public async Task SeedTestData() {
        if (await context.Hotels.AnyAsync()) {
            return;
        }

        var hotels = SeedData.CreateHotels();

        context.Hotels.AddRange(hotels);
        await context.SaveChangesAsync();

        var bookings = SeedData.CreateBookings(hotels);

        context.Bookings.AddRange(bookings);
        await context.SaveChangesAsync();
    }

    public async Task ResetTestData() {
        context.Bookings.RemoveRange(await context.Bookings.ToListAsync());
        context.Rooms.RemoveRange(await context.Rooms.ToListAsync());
        context.Hotels.RemoveRange(await context.Hotels.ToListAsync());

        await context.SaveChangesAsync();
    }
}