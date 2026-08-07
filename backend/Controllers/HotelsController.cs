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
}
