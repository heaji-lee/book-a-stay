using Microsoft.AspNetCore.Mvc;
using BookAStay.Services;
using BookAStay.Repositories.Models;

namespace BookAStay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController(HotelsService hotelsService) : ControllerBase {

    // GET: api/hotels?name={hotelName}
    [HttpGet]
    public async Task<IActionResult> GetHotelsByName([FromQuery] string? name) {
        var hotels = await hotelsService.GetHotelsByName(name);

        return hotels is null || hotels.Count == 0 ? NotFound("No hotels found.") : Ok(hotels);
    }

    // GET: api/hotels?checkIn={checkIn}&checkOut={checkOut}&guests={numberOfGuests}
    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms(
      [FromQuery] DateTime CheckInDate,
      [FromQuery] DateTime CheckOutDate,
      [FromQuery] int guestCount,
      [FromQuery] SortDirection sortDirection = SortDirection.Ascending
      ) {
        if (CheckInDate == default || CheckOutDate == default || CheckOutDate <= CheckInDate || guestCount <= 0) {
            return BadRequest("Provide valid dates and a guest count greater than 0.");
        }

        var availableRooms = await hotelsService.GetAvailableRooms(CheckInDate, CheckOutDate, guestCount, sortDirection);

        return availableRooms is null || availableRooms.Count == 0 ? NotFound("No available rooms found for the specified criteria.") : Ok(availableRooms);
    }
}
