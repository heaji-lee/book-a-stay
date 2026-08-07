using BookAStay.Repositories;
using BookAStay.Repositories.Models;
using System.Security.Cryptography;

namespace BookAStay.Services;

public class BookingsService(BookingsRepository bookingsRepository) {
    private const string ReferenceCharacters = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";

    public async Task<BookingDto?> GetBooking(string referenceNumber) {
        var booking = await bookingsRepository.GetBooking(referenceNumber);
        return booking;
    }

    public async Task<BookingDto> CreateBooking(
      int hotelId,
      int roomId,
      string guestName,
      int guestCount,
      DateTime checkInDate,
      DateTime checkOutDate
    ) {
        ValidateBookingRequest(guestName, guestCount, checkInDate, checkOutDate);

        var room = await bookingsRepository.GetRoomForBooking(roomId);
        if (room is null) {
            throw new ArgumentException("Selected room is not available.");
        }

        ValidateRoomSelection(room, hotelId, guestCount);
        ValidateNoRoomOverlap(room, checkInDate, checkOutDate);

        var nights = (checkOutDate - checkInDate).Days;
        var newBooking = new Booking {
            BookingReference = await GenerateUniqueBookingReference(),
            HotelId = hotelId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            GuestCount = guestCount,
            GuestName = guestName,
            TotalPrice = room.Price * nights
        };

        await bookingsRepository.CreateBooking(newBooking);

        return await bookingsRepository.GetBooking(newBooking.BookingReference)
            ?? throw new InvalidOperationException("Failed to retrieve the newly created booking.");
    }

    private static void ValidateBookingRequest(string guestName, int guestCount, DateTime checkInDate, DateTime checkOutDate) {
        if (string.IsNullOrWhiteSpace(guestName))
            throw new ArgumentException("Guest name is required.");

        if (guestCount <= 0)
            throw new ArgumentException("Guest count must be greater than zero.");

        if (checkInDate.Date < DateTime.Today)
            throw new ArgumentException("Check-in date cannot be in the past.");

        if (checkInDate >= checkOutDate)
            throw new ArgumentException("Check-in date must be before check-out date.");
    }

    private static void ValidateRoomSelection(Room room, int hotelId, int guestCount) {
        if (room is null)
            throw new ArgumentException("Selected room is not available.");

        if (room.HotelId != hotelId)
            throw new ArgumentException("Selected room does not belong to the specified hotel.");

        if (room.Capacity < guestCount)
            throw new ArgumentException("Selected room cannot accommodate the specified number of guests.");
    }

    private static void ValidateNoRoomOverlap(Room room, DateTime checkInDate, DateTime checkOutDate) {
        var overlapsExistingBooking = room.Bookings.Any(booking =>
            booking.CheckInDate < checkOutDate &&
            booking.CheckOutDate > checkInDate);

        if (overlapsExistingBooking)
            throw new InvalidOperationException("The selected room is already booked for the specified dates.");
    }

    private async Task<string> GenerateUniqueBookingReference() {
        string reference;
        do {
            reference = GenerateBookingReference();
        }
        while (await bookingsRepository.BookingReferenceExists(reference));
        return reference;
    }

    private static string GenerateBookingReference() {
        var characters = new char[6];

        for (var index = 0; index < characters.Length; index++) {
            characters[index] = ReferenceCharacters[
                RandomNumberGenerator.GetInt32(ReferenceCharacters.Length)];
        }

        return new string(characters);
    }
}
