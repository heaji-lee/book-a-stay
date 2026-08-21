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

        if (string.IsNullOrWhiteSpace(name)) {
            return Ok(hotels ?? new List<HotelDto>());
        }

        return hotels is null || hotels.Count == 0
            ? NotFound("No hotels found for the specified search term.")
            : Ok(hotels);
    }

    // GET: api/hotels?checkIn={checkIn}&checkOut={checkOut}&guests={numberOfGuests}
    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms(
      [FromQuery] string? hotel,
      [FromQuery] DateTime checkInDate,
      [FromQuery] DateTime checkOutDate,
      [FromQuery] int guestCount,
      [FromQuery] SortDirection sortDirection = SortDirection.Ascending
      ) {
        if (checkInDate == default || checkOutDate == default || checkOutDate <= checkInDate || guestCount <= 0) {
            return BadRequest("Provide valid dates and a guest count greater than 0.");
        }

        var availableRooms = await hotelsService.GetAvailableRooms(hotel, checkInDate, checkOutDate, guestCount, sortDirection);

        return availableRooms is null || availableRooms.Count == 0 ? NotFound("No available rooms found for the specified criteria.") : Ok(availableRooms);
    }
}
