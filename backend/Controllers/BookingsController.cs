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
}
