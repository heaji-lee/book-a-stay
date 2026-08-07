using BookAStay.Repositories;
using BookAStay.Repositories.Models;
using System.Security.Cryptography;

namespace BookAStay.Services;

public class BookingsService(BookingsRepository bookingsRepository) {

    public async Task<BookingDto?> GetBooking(string referenceNumber) {
        var booking = await bookingsRepository.GetBooking(referenceNumber);
        return booking;
    }
}
