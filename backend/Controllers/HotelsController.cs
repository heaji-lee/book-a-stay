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

        return Ok(hotels);
    }

    // GET: api/hotels?checkIn={checkIn}&checkOut={checkOut}&guests={numberOfGuests}
    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms(
      [FromQuery] DateTime CheckInDate,
      [FromQuery] DateTime CheckOutDate,
      [FromQuery] int guests,
      [FromQuery] SortDirection sortDirection = SortDirection.Ascending
      ) {
        if (CheckInDate == default || CheckOutDate == default || CheckOutDate <= CheckInDate || guests <= 0) {
            return BadRequest("Provide valid dates and a guest count greater than 0.");
        }

        var availableRooms = await hotelsService.GetAvailableRooms(CheckInDate, CheckOutDate, guests, sortDirection);
        return Ok(availableRooms);
    }
}
