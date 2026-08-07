using Microsoft.AspNetCore.Mvc;
using BookAStay.Services;
using BookAStay.Repositories.Models;

namespace BookAStay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(BookingsService bookingsService) : ControllerBase {

    // GET: api/bookings?referenceNumber={referenceNumber}
    [HttpGet("{referenceNumber}")]
    public async Task<IActionResult> GetBooking(
      [FromRoute] string referenceNumber = ""
      ) {
        var booking = await bookingsService.GetBooking(referenceNumber);

        return booking is null ? NotFound($"Booking with reference number '{referenceNumber}' not found.") : Ok(booking);
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDto bookingRequest) {
        try {
            var hotelId = bookingRequest.HotelId;
            var roomId = bookingRequest.RoomId;
            var guestName = bookingRequest.GuestName;
            var guestCount = bookingRequest.GuestCount;
            var checkInDate = bookingRequest.CheckInDate;
            var checkOutDate = bookingRequest.CheckOutDate;
            var createdBooking = await bookingsService.CreateBooking(hotelId, roomId, guestName, guestCount, checkInDate, checkOutDate);
            return Ok(createdBooking);
        }
        catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex) {
            return Conflict(ex.Message);
        }
    }
}
