using Microsoft.EntityFrameworkCore;
using BookAStay.Data;
using BookAStay.Repositories.Models;

namespace BookAStay.Repositories;

public class BookingsRepository {
    private readonly AppDbContext _context;

    public BookingsRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<BookingDto?> GetBooking(string bookingReference) {
        return await _context.Bookings
            .AsNoTracking()
            .Where(booking => booking.BookingReference == bookingReference)
            .Select(booking => new BookingDto {
                Id = booking.Id,
                BookingReference = booking.BookingReference,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                RoomId = booking.RoomId,
                RoomType = booking.Room.Type,
                HotelId = booking.Room.HotelId,
                HotelName = booking.Room.Hotel.Name,
                TotalPrice = booking.TotalPrice,
                GuestName = booking.GuestName,
                GuestCount = booking.GuestCount
            })
            .FirstOrDefaultAsync();
    }

    public async Task CreateBooking(Booking booking) {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<Room?> GetRoomForBooking(int roomId) {
        return await _context.Rooms
            .AsNoTracking()
            .Include(room => room.Bookings)
            .FirstOrDefaultAsync(room => room.Id == roomId);
    }

    public async Task<bool> BookingReferenceExists(string bookingReference) {
        return await _context.Bookings
            .AsNoTracking()
            .AnyAsync(b => b.BookingReference == bookingReference);
    }
}
