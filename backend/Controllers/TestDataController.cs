using Microsoft.AspNetCore.Mvc;
using BookAStay.Services;

namespace BookAStay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestDataController(TestDataService testDataService) : ControllerBase {

    // POST: api/testdata/seed
    [HttpPost("seed")]
    public async Task<IActionResult> SeedTestData() {
        await testDataService.SeedTestData();
        return Ok("Test data seeded successfully.");
    }

    // DELETE: api/testdata/reset
    [HttpDelete("reset")]
    public async Task<IActionResult> ResetTestData() {
        await testDataService.ResetTestData();
        return Ok("Test data reset successfully.");
    }
}
